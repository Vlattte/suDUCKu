using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class Cell : MonoBehaviour
{
    public CellStruct cellData;
    //values
    private int mainValue;
    private int userValue;
    private bool[] noteValuesInCell;        //first element to check existence of any numbers        
    public NotesController notesController; //set number in note cells

    //number images
    public Sprite[] imageNumber;

    //positions
    private int gridPosX;
    private int gridPosY;

    //UI
    public TextMeshProUGUI textNumber;
    public Image backImage;
    public bool isEmpty;
    private bool isHintCell;
    private int isAnyNotes;

    //Grid controller
    private SuduckuGrid GridManager;

    //Image colors
    private Color notActiveColor;
    private Color activeColor;

    //text colors
    private Color mistakeColor;
    private Color notChangableNumberColor;
    private Color changableNumberColor;
    private Color hintColor;

    private void Awake()
    {
        notChangableNumberColor = new Color(99 / 255f, 99 / 255f, 99 / 255f);
        hintColor = new Color(0, 0, 1);
        changableNumberColor = new Color(0, 0, 0);

        notActiveColor = Color.white;
        activeColor = new Color(0.6556604f, 0.896965f, 1);
        mistakeColor = new Color(0.5960785f, 0, 0, 1);
    }

    private void Start()
    {
        GridManager = GameObject.FindGameObjectWithTag("GridManager").GetComponent<SuduckuGrid>();

        if(userValue != 0)
        {
            return;
        }
        noteValuesInCell = new bool[9];

        for (int i = 0; i < 9; i++)
            noteValuesInCell[i] = false;

        isAnyNotes = 0;
        userValue = 0;

        cellData.noteValuesInCell = noteValuesInCell;
        cellData.isAnyNotes = 0;
        cellData.userValue = 0;
        cellData.mainValue = mainValue;
    }

    public void ParseCellStruct(CellStruct _data)
    {
        cellData = _data;
        mainValue = _data.mainValue;
        userValue = _data.userValue;
        noteValuesInCell = _data.noteValuesInCell;
        gridPosX = _data.gridPosX;
        gridPosY = _data.gridPosY;
        isEmpty = _data.isEmpty;
        isAnyNotes = _data.isAnyNotes;
        isHintCell = _data.isHintCell;
        ReturnSaveDataInCell();
    }

    public Cell(int _gridPosX, int _gridPosY)
    {
        gridPosX = _gridPosX;
        gridPosY = _gridPosY;
    }

    public int GetMainValue
    {
        get { return mainValue; }
    }

    public int ManageUserValue
    {
        get { return userValue; }
        set 
        { 
            userValue = value;
            cellData.userValue = userValue;
        }
    }

    public bool MatchUserAndMainValues
    {
        get { return (userValue == mainValue); }
    }

    public void SetGridPos(int _gridPosX, int _gridPosY)
    {
        gridPosX = _gridPosX;
        gridPosY = _gridPosY;

        cellData.gridPosX = gridPosX;
        cellData.gridPosY = gridPosY;
    }

    public int[] GetGridPos()
    {

        int[] pos = new int[2];
        pos[0] = gridPosX;
        pos[1] = gridPosY;
        return pos;
    }

   /* public Vector2 ManageWorldPos
    {
        get { return worldPos; }
        set { worldPos = value; }
    }*/

    //returns data in cell from save binary file
    //set back uservalue, mainvalue, notes, text color(mistake, hint, not changeble or changable),
    public void ReturnSaveDataInCell()
    {
        if(isEmpty)
        {
            if (userValue != 0)
            {
                if (userValue != mainValue)
                {
                    textNumber.text = userValue.ToString();
                    textNumber.color = mistakeColor;
                }
                else
                {
                    textNumber.text = userValue.ToString();
                    textNumber.color = changableNumberColor;
                }
            }
            else if(isAnyNotes > 0)
            {
                for(int i = 0; i < 9; i++)
                {
                    if(noteValuesInCell[i] == true)
                        notesController.SetNoteActive(true, i+1);
                }
            }

            //if hint => change text color to hint color
            if (cellData.isHintCell == true)
                textNumber.color = hintColor;
        }
        else 
        {
            
            textNumber.text = mainValue.ToString();
            textNumber.color = notChangableNumberColor;
        }
    }

    public void RememberNumber(int number)
    {
        mainValue = number;
        cellData.mainValue = number;
    }

    //-number to empty cell and mainvalue = number
    //number to change main number to number
    public void SetNumberUI(int number)
    {
        
        if (number == 0)
        {
            isEmpty = true;
            textNumber.text = " ";
            textNumber.color = changableNumberColor;

            cellData.isEmpty = true;
        }
        else if (number < 0)
        {
            //method is used for Empty cell, when sudocku is creating
            mainValue = -1 * number;
            isEmpty = true;
            textNumber.text = " ";
            textNumber.color = changableNumberColor;
            cellData.isEmpty = true;
            cellData.mainValue = mainValue;
        }
        else
        {
            if (isAnyNotes > 0)
            {
                ClearCell();
                isAnyNotes = 0;
                cellData.isAnyNotes = 0;
            }

            mainValue = number;
            isEmpty = false;
            textNumber.text = mainValue.ToString();
            textNumber.color = notChangableNumberColor;

            cellData.mainValue = number;
            cellData.isEmpty = false;
            /*if (number > 3)
            {
                textNumber.text = mainValue.ToString();
                textNumber.color = notChangableNumber;
            }
            else 
            {
                backImage.sprite = imageNumber[number - 1];
            }*/
        }
    }

    //number range [1, 9]
    //returns  1  if number == mainvalue
    //returns -1 if number != mainvalue
    //returns  0  if number == 0 or number == userValue
    public int SetUserNumber(int number)
    {
        if (isEmpty)
        {
            if (userValue == number)
            {
                ClearCell();
                return 0;
            }
            else 
            {
                //if there is any user notes, 
                //clear them and write number
                if (isAnyNotes > 0)
                    ClearCell();

                userValue = number;
                cellData.userValue = number;

                //if number isn't right increase mistakes
                //and set mistake color
                if (userValue != mainValue)
                {
                    textNumber.color = mistakeColor;
                    GridManager.ManageMistakesCount();
                    textNumber.text = userValue.ToString();
                    cellData.userValue = number;
                    return -1;
                }
                //if there was a mistake => change color and set number
                else if (textNumber.color == mistakeColor && userValue == mainValue)
                {
                    textNumber.color = changableNumberColor;
                }
                textNumber.text = userValue.ToString();
                cellData.userValue = number;
                return 1;
            } 
        }
        return 0;
    }

    public void SetHint()
    {
        if (isAnyNotes > 0)
        {
            for (int i = 0; i < 9; i++)
            {
                notesController.SetNoteActive(false, i + 1);
                noteValuesInCell[i] = false;
                cellData.noteValuesInCell[i] = false;
            }
            isAnyNotes = 0;

            cellData.isAnyNotes = 0;
        }
        textNumber.text = mainValue.ToString();
        textNumber.color = hintColor;
        userValue = mainValue;
        isHintCell = true;

        cellData.isHintCell = true;
        cellData.userValue = mainValue;
    }

    //number range [1, 9]
    public void SetLittleNumber(int number)
    {
        if (isEmpty)
        {
            if (userValue != 0)
            {
                ClearCell();
            }

            //Color temp_color = hintValues[number - 1].GetComponent<Image>().color;
            if (noteValuesInCell[number-1] == true)
            {
                noteValuesInCell[number-1] = false;
                //hintValues[number - 1].GetComponent<Image>().color = new Color(temp_color.r, temp_color.g, temp_color.b, 0f);
                notesController.SetNoteActive(false, number);
                isAnyNotes--;
                cellData.isAnyNotes--;
            }
            else
            {
                noteValuesInCell[number-1] = true;
                //hintValues[number - 1].GetComponent<Image>().color = new Color(temp_color.r, temp_color.g, temp_color.b, 1f);
                notesController.SetNoteActive(true, number);
                isAnyNotes++;

                cellData.noteValuesInCell[number - 1] = true;
                cellData.isAnyNotes++;
            }
        }
    }

    private void OnMouseDown()
    {
        GridManager.SetActiveCell(gridPosX, gridPosY);
    }

    public void SetCellActive(bool isMakeActive)
    {
        if (isMakeActive)
            backImage.color = activeColor;
        else
            backImage.color = notActiveColor; 
    }

    public void ClearCellInRestart()
    {
        textNumber.text = " ";
        if (isAnyNotes > 0)
        {
            for (int i = 0; i < 9; i++)
            {
                notesController.SetNoteActive(false, i + 1);
                noteValuesInCell[i] = false;
                cellData.noteValuesInCell[i] = false;
            }
            isAnyNotes = 0;

            cellData.isAnyNotes = 0;
            cellData.isHintCell = false;
        }
    }

    public void ClearCell()
    {
        if (isEmpty)
        {
            //if GetEnterModeStatus == true
            //big numbers
            textNumber.text = " ";
            backImage.color = activeColor;
            if (isAnyNotes > 0)
            {
                for (int i = 0; i < 9; i++)
                {
                    notesController.SetNoteActive(false, i+1);
                    noteValuesInCell[i] = false;
                    cellData.noteValuesInCell[i] = false;
                }
                isAnyNotes = 0;
                cellData.isAnyNotes = 0;
            }
            userValue = 0;
            cellData.userValue = 0;
            cellData.isHintCell = false;
        }
    }
}
