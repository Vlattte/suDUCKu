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

    //positions
    public Vector2 worldPos;
    public int gridPosX;
    public int gridPosY;

    //UI
    public Text textNumber;
    public bool isEmpty;

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
        mainValue = number;
        if (mainValue == 0)
        {
            isEmpty = true;
            textNumber.text = " ";
        }
        else
        {
            isEmpty = false;
            textNumber.text = mainValue.ToString();
        }
    }

    private void OnMouseDown()
    {
        Debug.Log("“€ ");
    }

}
