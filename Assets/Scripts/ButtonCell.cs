using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonCell : MonoBehaviour
{
    private int buttonValue;
    private SuduckuGrid GridManager;
    public Text textNumber;
    public Image image;

    //sprites of numbers to show
    public Sprite[] imageNumbersArray;

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
            textNumber.gameObject.SetActive(false);
            image.gameObject.SetActive(true);
            image.sprite = imageNumbersArray[value - 1];
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
