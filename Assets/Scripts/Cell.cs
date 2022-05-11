using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class Cell : MonoBehaviour
{
    //values
    public  int mainValue;
    private int userValue;
    private bool[] hintValuesInCell;        //first element to check existence of any numbers        
    public NotesController notesController; //set number in note cells

    public Sprite[] imageNumber;

    //positions
    private Vector2 worldPos;
    private int gridPosX;
    private int gridPosY;

    //UI
    public TextMeshProUGUI textNumber;
    public Image backImage;
    public bool isEmpty;
    private int isAnyNotes;

    //Grid
    private SuduckuGrid GridManager;

    //Image colors
    private Color notActiveColor;
    private Color activeColor;
    private Color mistakeColor;

    //text colors
    private Color notChangableNumber;
    private Color changableNumber;

    private void Start()
    {
        GridManager = GameObject.FindGameObjectWithTag("GridManager").GetComponent<SuduckuGrid>();

        hintValuesInCell = new bool[9];
        for (int i = 0; i < 9; i++)
            hintValuesInCell[i] = false;

        notActiveColor = Color.white;
        activeColor = new Color(0.6556604f, 0.896965f, 1);
        mistakeColor = new Color(0.5960785f, 0, 0);

        notChangableNumber = new Color(99/255f, 99 / 255f, 99 / 255f);
        changableNumber = new Color(0, 0, 0);

        isAnyNotes = 0;
    }

    public Cell(int _gridPosX, int _gridPosY, Vector2 _worldPos)
    {
        gridPosX = _gridPosX;
        gridPosY = _gridPosY;
        worldPos = _worldPos;
    }

    public int GetMainValue
    {
        get { return mainValue; }
    }

    public int ManageUserValue
    {
        get { return userValue; }
        set { userValue = value; }
    }

    public bool MatchUserAndMainValues
    {
        get { return (userValue == mainValue); }
    }

    public void SetGridPos(int _gridPosX, int _gridPosY)
    {
        gridPosX = _gridPosX;
        gridPosY = _gridPosY;
    }

    public Vector2 ManageWorldPos
    {
        get { return worldPos; }
        set { worldPos = value; }
    }

    public void RememberNumber(int number)
    {
        mainValue = number;
    }

    //-number to empty cell and mainvalue = number
    //number to change main number to number
    public void SetNumberUI(int number)
    {
        
        if (number == 0)
        {
            isEmpty = true;
            textNumber.text = " ";
            textNumber.color = changableNumber;
        }
        else if (number < 0)
        {
            //method is used for Empty cell, when sudocku is creating
            mainValue = -1 * number;
            isEmpty = true;
            textNumber.text = " ";
            textNumber.color = changableNumber;
        }
        else
        {
            if (isAnyNotes > 0)
            {
                ClearCell();
                isAnyNotes = 0;
            }

            mainValue = number;
            isEmpty = false;
            textNumber.text = mainValue.ToString();
            textNumber.color = notChangableNumber;
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
    public void SetUserNumber(int number)
    {
        if (isEmpty)
        {
            if (userValue == number)
                ClearCell();
            else 
            {
                //if there is any user notes, 
                //clear them and write number
                if (isAnyNotes > 0)
                    ClearCell();

                userValue = number;
                
                //if number isn't right increase mistakes
                //and set mistake color
                if (userValue != mainValue)
                {
                    textNumber.color = mistakeColor;
                    GridManager.ManageMistakesCount();
                }
                else if (textNumber.color == mistakeColor && userValue == mainValue)
                    textNumber.color = changableNumber;

                textNumber.text = userValue.ToString();
            } 
        }
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
            if (hintValuesInCell[number-1] == true)
            {
                hintValuesInCell[number-1] = false;
                //hintValues[number - 1].GetComponent<Image>().color = new Color(temp_color.r, temp_color.g, temp_color.b, 0f);
                notesController.SetNoteActive(false, number);
                isAnyNotes--;
            }
            else
            {
                hintValuesInCell[number-1] = true;
                //hintValues[number - 1].GetComponent<Image>().color = new Color(temp_color.r, temp_color.g, temp_color.b, 1f);
                notesController.SetNoteActive(true, number);
                isAnyNotes++;
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
                    hintValuesInCell[i] = false;
                }
                isAnyNotes = 0;
            }
            userValue = 0;
        }
    }
}
