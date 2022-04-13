using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Cell : MonoBehaviour
{
    //values
    private int mainValue;
    private int userValue;
    private int hintValue;
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
        
        notActiveColor = Color.white;
        activeColor = new Color(0.6556604f, 0.896965f, 1);
        mistakeColor = new Color(0.5960785f, 0, 0);

        notChangableNumber = new Color(0, 0, 0);
        changableNumber = new Color(0.1960784f, 0.1960784f, 0.1960784f);
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
                    backImage.color = mistakeColor;
                else if (backImage.color == mistakeColor && userValue == mainValue)
                    backImage.color = activeColor;

                textNumber.text = userValue.ToString();
            } 
        }
    }

    public void SetLittleNumber(int number)
    {
        if(hintValue == number)
        {
            hintValues[number - 1].SetActive(false);
        }
        else
        {
            hintValue = number;

            if (hintValue == 0)
            {
                Debug.Log("HOW DID YOU ENTER ZERO?!");
                return;
            }
            else
            {
                hintValues[number - 1].SetActive(true);
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
        }
    }
}
