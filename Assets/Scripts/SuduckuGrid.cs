using System.Collections.Generic;
using UnityEngine;


public class SuduckuGrid : MonoBehaviour
{
    //cell positions
    public float gridSizeX, gridSizeY;      // cell position in grid format: [x, y]
    public float cellRadius;

    public List<GameObject> suduckuTable;   // list of cells as game objects
    public List<GameObject> buttons;        // buttons, which enter numbers in cells
    public GameObject cellPrefab;           
    public SudokuGenerator generator;       // sudoku numbers generator
    public HeartConroller lives;            // lives manager

    public GameObject WinPanelObj;          // Panel, that will apear after win
   
    public bool isGenerated;                // bool for EnterCells to begin spawning buttons
    public bool isEnterBigNumbers;          // enter mode: is user enter notes or big numbers in cell

    public int countRightNumbers;           // count number of right filled cells, if equal to 81, game is won
    private int activeCellX, activeCellY;   // position of hightlighted cell (suducku table positions: from 0 to 80)

    private void Start()
    {
        //not filled cells
        countRightNumbers = 81 - DataHolder.ManageDifficulty;

        //none of cells is highlited
        activeCellX = -1;
        activeCellY = -1;

        //isGenerated = false;
        isEnterBigNumbers = true;
        SetIndexes();
    }

    void SetIndexes()
    {
        for (int y = 0; y < 9; y++)
        {
            for (int x = 0; x < 9; x++)
            {
                Cell cellRef = cellPrefab.GetComponent<Cell>();
                suduckuTable[x + 9 * y].GetComponent<Cell>().SetGridPos(x, y);
            }
        }
    }

    //Count number of right cells
    public void ChangeRightNumberCount(int _isNumberRight)
    {
        countRightNumbers += _isNumberRight;
        if (countRightNumbers == 81)
        {
            WinPanelObj.SetActive(true);
            return;
        }
    }

    //Increase mistakes and change one heart to gray
    public void ManageMistakesCount()
    {
        lives.decreaseLives();
    }

    //Highlight cell
    public void SetActiveCell(int x, int y)
    {
        if (activeCellX != -1)
        {
            for (int i = 0; i < 9; i++)
            {
                suduckuTable[activeCellX + 9 * i].GetComponent<Cell>().SetCellActive(false);
                suduckuTable[i + activeCellY * 9].GetComponent<Cell>().SetCellActive(false);
            }
        }


        activeCellX = x;
        activeCellY = y;
        for (int i = 0; i < 9; i++)
        {
            suduckuTable[activeCellX + 9 * i].GetComponent<Cell>().SetCellActive(true);
            suduckuTable[i + activeCellY * 9].GetComponent<Cell>().SetCellActive(true);
        }
    }

    //Set notes or final number
    public void ChangeEnterMode()
    {
        isEnterBigNumbers = !isEnterBigNumbers;
    }

    //Get enter mode
    public bool GetEnterModeStatus
    {
        get { return isEnterBigNumbers; }
    }

    //user change numbers functions
    public void SetNumberInCell(int value)
    {
        if(activeCellX != -1)
        {
            int isRightNumber = suduckuTable[activeCellX + activeCellY * 9].GetComponent<Cell>().SetUserNumber(value);
            ChangeRightNumberCount(isRightNumber);
        }
    }

    //Set note = value
    public void SetLittleNumberInCell(int value)
    {
        if (activeCellX != -1 && activeCellY != -1)
        {
            suduckuTable[activeCellX + activeCellY * 9].GetComponent<Cell>().SetLittleNumber(value);
        }
    }

    //Set cell empty
    public void ClearCell()
    {
        suduckuTable[activeCellX + activeCellY * 9].GetComponent<Cell>().ClearCell();
    }
    ///////////////////////////////
}
