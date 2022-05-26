using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class CellStruct
{
    public int mainValue;
    public int userValue;
    public bool[] noteValuesInCell;        //first element to check existence of any numbers   

    public int gridPosX;
    public int gridPosY;

    public bool isEmpty;
    public int isAnyNotes;
    public bool isHintCell;
}
