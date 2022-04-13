using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonCell : MonoBehaviour
{
    private int buttonValue;
    private SuduckuGrid GridManager;
    public Text textNumber;

    private void Start()
    {
        GridManager = GameObject.FindGameObjectWithTag("GridManager").GetComponent<SuduckuGrid>();
    }

    private void OnMouseDown()
    {
        if (GridManager.GetEnterModeStatus)
            SetNumberInCell();
        else
            SetLittleNumberInCell();
    }

    public int SetButtonValue
    {
        set 
        { 
            buttonValue = value;
            textNumber.text = buttonValue.ToString();
        }
    }

    public void SetNumberInCell()
    {
        GridManager.SetNumberInCell(buttonValue);
    }

    public void SetLittleNumberInCell()
    {
        GridManager.SetLittleNumberInCell(buttonValue);
    }
}
