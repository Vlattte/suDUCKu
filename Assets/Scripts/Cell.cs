using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Cell : MonoBehaviour
{
    //values
    private int mainValue;
    private int userValue;
    private bool[] hintValuesInCell; //first element to check existence of any numbers
    public GameObject[] hintValues;

    public Sprite[] imageNumber;

    //positions
    private Vector2 worldPos;
    private int gridPosX;
    private int gridPosY;

    //UI
    public Text textNumber;
    public Image backImage;
    public bool isEmpty;

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

        hintValuesInCell = new bool[10];
        for (int i = 0; i < 10; i++)
            hintValuesInCell[i] = false;

        notActiveColor = Color.white;
        activeColor = new Color(0.6556604f, 0.896965f, 1);
        mistakeColor = new Color(0.5960785f, 0, 0);

        notChangableNumber = new Color(0.4056604f, 0.2034299f, 0.0478373f);
        changableNumber = new Color(0, 0, 0);
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

    public void SetNumber(int number)
    {
        
        if (number == 0)
        {
            isEmpty = true;
            textNumber.text = " ";
            textNumber.color = changableNumber;
        }
        else
        {
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

    public void SetUserNumber(int number)
    {
        if (isEmpty)
        {
            if (userValue == number)
                ClearCell();
            else 
            {
                userValue = number;
                if (userValue != mainValue)
                    textNumber.color = mistakeColor;
                else if (textNumber.color == mistakeColor && userValue == mainValue)
                    textNumber.color = changableNumber;

                textNumber.text = userValue.ToString();
            } 
        }
    }

    public void SetLittleNumber(int number)
    {
        if (userValue != 0)
        {
            ClearCell();
        }

        Color temp_color = hintValues[number - 1].GetComponent<Image>().color;
        if (hintValuesInCell[number] == true)
        {
            hintValues[number - 1].GetComponent<Image>().color = new Color(temp_color.r, temp_color.g, temp_color.b, 0f);
        }
        else
        {
            hintValuesInCell[number] = true;
            hintValues[number - 1].GetComponent<Image>().color = new Color(temp_color.r, temp_color.g, temp_color.b, 1f);
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
            if (GridManager.GetEnterModeStatus)
            {
                textNumber.text = " ";
                backImage.color = activeColor;
            }
            else
            {
                foreach (GameObject note in hintValues)
                {
                    note.SetActive(false);
                }
            }
            userValue = 0;
        }
    }
}
