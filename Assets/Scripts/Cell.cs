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
    public NotesController notesController; //set number in note cells

    //number images
    public Sprite[] imageNumber;

    //UI
    public TextMeshProUGUI textNumber;
    public Image backImage;

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

        activeColor = new Color(0.6556604f, 0.896965f, 1);
        notActiveColor = Color.white;
        mistakeColor = new Color(0.5960785f, 0, 0);
    }

    private void Start()
    {
        GridManager = GameObject.FindGameObjectWithTag("GridManager").GetComponent<SuduckuGrid>();

        if(!DataHolder.IsContinueMode)
        {
            cellData.noteValuesInCell = new bool[9];
            for (int i = 0; i < 9; i++)
                cellData.noteValuesInCell[i] = false;
            cellData.isAnyNotes = 0;
            cellData.userValue = 0;
        }
    }

    public void ParseCellStruct(CellStruct _data)
    {
        cellData = _data;
        ReturnSaveDataInCell();
    }

    public bool GetIsEmpty
    {
        get { return cellData.isEmpty; }
    }

    public int GetMainValue
    {
        get { return cellData.mainValue; }
    }

    public int ManageUserValue
    {
        get { return cellData.userValue; }
        set { cellData.userValue = value; }
    }

    public void SetGridPos(int _gridPosX, int _gridPosY)
    {
        cellData.gridPosX = _gridPosX;
        cellData.gridPosY = _gridPosY;
    }

    //set number in ui text mesh pro object
    void ChangeTextNumber(string _number, Color _color)
    {
        textNumber.text = _number.ToString();
        textNumber.color = _color;
    }

    //returns data in cell from save binary file
    //set back uservalue, mainvalue, notes, text color(mistake, hint, not changeble or changable),
    public void ReturnSaveDataInCell()
    {
        if(cellData.isEmpty)
        {
            //if user value was in cell 
            if (cellData.userValue != 0)
            {
                if (cellData.userValue != cellData.mainValue)
                {
                    //if there was a mistake
                    ChangeTextNumber(cellData.userValue.ToString(), new Color(0.5960785f, 0, 0));
                }
                else
                {
                    //if user value was equal to main value
                    ChangeTextNumber(cellData.userValue.ToString(), new Color(0, 0, 0));
                }
            }
            else if(cellData.isAnyNotes > 0)
            {
                for(int i = 0; i < 9; i++)
                {
                    if(cellData.noteValuesInCell[i] == true)
                        notesController.SetNoteActive(true, i+1);
                }
            }

            //if hint => change text color to hint color
            if (cellData.isHintCell == true)
                ChangeTextNumber(cellData.mainValue.ToString(), new Color(0, 0, 1));
        }
        else 
        {
            //if cell was filled while generating
            ChangeTextNumber(cellData.mainValue.ToString(), new Color(99 / 255f, 99 / 255f, 99 / 255f));
        }
    }

    //-number to empty cell and mainvalue = number
    //number to change main number to number
    public void SetNumberUI(int number)
    {
        if (number == 0)
        {
            ChangeTextNumber(" ", changableNumberColor);
            cellData.isEmpty = true;
        }
        else if (number < 0)
        {
            //method is used for Empty cell, when sudocku is creating
            //set empty cell, but remember this main value
            cellData.mainValue = -1 * number;
            cellData.isEmpty = true;

            ChangeTextNumber(" ", changableNumberColor);
        }
        else
        {
            if (cellData.isAnyNotes > 0)
                ClearNotes();

            //set cell with shown main number, so it is not empty
            cellData.mainValue = number;
            cellData.isEmpty = false;

            ChangeTextNumber(cellData.mainValue.ToString(), notChangableNumberColor);
        }
    }


    //number range [1, 9]
    //returns  1  if number == mainvalue
    //returns -1 if number != mainvalue
    //returns  0  if number == 0 or number == userValue
    public int SetUserNumber(int number)
    {
        if (cellData.isEmpty)
        {
            if (cellData.userValue == number)
            {
                ClearCell();
            }
            else 
            {
                //if there is any user notes, 
                //clear them and write number
                if (cellData.isAnyNotes > 0)
                    ClearNotes();

                cellData.userValue = number;

                //if number isn't right increase mistakes
                //and set mistake color
                if (cellData.userValue != cellData.mainValue)
                {
                    ChangeTextNumber(cellData.userValue.ToString(), mistakeColor);
                    return -1;
                }
                else
                {
                    ChangeTextNumber(cellData.userValue.ToString(), changableNumberColor);
                    return 1;
                }
            } 
        }
        return 0;
    }

    //number range [1, 9]
    public void SetLittleNumber(int number)
    {
        if (cellData.isEmpty)
        {
            if (cellData.userValue != 0)
                ClearCell();

            if (cellData.noteValuesInCell[number-1] == true)
            {
                //if this note is already in this cell, hide it 
                notesController.SetNoteActive(false, number);
  
                cellData.isAnyNotes--;
                cellData.noteValuesInCell[number - 1] = false;
            }
            else
            {
                //if there is no note in this cell, show it
                notesController.SetNoteActive(true, number);

                cellData.noteValuesInCell[number - 1] = true;
                cellData.isAnyNotes++;
            }
        }
    }

    public void SetHint()
    {
        if (cellData.isAnyNotes > 0)
            ClearNotes();
        ChangeTextNumber(cellData.mainValue.ToString(), hintColor);

        cellData.userValue = cellData.mainValue;
        cellData.isHintCell = true;
    }

    private void OnMouseDown()
    {
        GridManager.SetActiveCell(cellData.gridPosX, cellData.gridPosY);
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
        ChangeTextNumber(" ", changableNumberColor);
        if (cellData.isAnyNotes > 0)
            ClearNotes();
    }

    public void ClearCell()
    {
        if (cellData.isEmpty)
        {
            //if GetEnterModeStatus == true
            //big numbers
            ChangeTextNumber(" ", activeColor);
            if (cellData.isAnyNotes > 0)
                ClearNotes();
            
            cellData.userValue = 0;
            cellData.isHintCell = false;
        }
    }

    private void ClearNotes()
    {
        for (int i = 0; i < 9; i++)
        {
            notesController.SetNoteActive(false, i+1);        
            cellData.noteValuesInCell[i] = false;
        }

        cellData.isAnyNotes = 0;
        cellData.isHintCell = false;
    }
}
