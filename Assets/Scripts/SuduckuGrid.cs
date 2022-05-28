using System.Collections.Generic;
using UnityEngine;
using System.IO;



public class SuduckuGrid : MonoBehaviour
{
    [SerializeField] private List<GameObject> sudokuTable;   // list of cells as game objects
    public HeartConroller lives;            // lives manager

    public GameObject WinPanelObj;          // Panel, that will apear after win
    public GameObject LosePanelObj;         // Panel, that will apear after losw

    private bool isEnterBigNumbers;         // enter mode: is user enter notes or big numbers in cell (false for notes)

    private int countRightNumbers;          // count number of right filled cells, if equal to 81, game is won
    private int hintCount;                  // number of available hints
    private int activeCellX, activeCellY;   // position of hightlighted cell (suducku table positions: from 0 to 80)


    //save data
    [SerializeField] private CellStruct[] cells;  //buffer of cells
    [SerializeField] private string saveFilePath; //saved data path

    private void Awake()
    {
        //saved data path
        saveFilePath = Application.persistentDataPath + "/Save.dat";

        //load save, if continue pressed
        if (DataHolder.IsContinueMode)
        {
            cells = SaveSudoku.LoadData<CellStruct[]>(saveFilePath);
            CellsToSudokuTable();
            //File.Delete(saveFilePath);
        }
    }

    private void Start()
    {
        //if continue mode
        if(DataHolder.IsContinueMode == true)
        {
            //number of not filled cells
            countRightNumbers = PlayerPrefs.GetInt("countRightNumbers");
            hintCount = PlayerPrefs.GetInt("hintCount");
            for (int i = 0; i < 3 - PlayerPrefs.GetInt("mistakesCount"); i++)
                lives.decreaseLives();
        }
        else
        {
            countRightNumbers = 81 - DataHolder.ManageDifficulty;
            hintCount = 3;

            //save data buffer
            cells = new CellStruct[81];
            SetIndexes();
        }
        PlayerPrefs.DeleteAll();

        //if none of cells is highlited
        activeCellX = -1;
        activeCellY = -1;


        //isGenerated = false;
        isEnterBigNumbers = true;
    }

    private void OnApplicationQuit()
    {
        SaveCurCells();
    }

    public void SaveCurCells()
    {
        PlayerPrefs.SetInt("countRightNumbers", countRightNumbers);
        PlayerPrefs.SetInt("mistakesCount", lives.GetLives());
        PlayerPrefs.SetInt("hintCount", hintCount);

        for (int i = 0; i < 81; i++)
        {
            cells[i] = sudokuTable[i].GetComponent<Cell>().cellData;
        }

        if (cells != null)
            SaveSudoku.SaveData(saveFilePath, cells);
    }

    //set grid positions of cells
    void SetIndexes()
    {
        for (int y = 0; y < 9; y++)
        {
            for (int x = 0; x < 9; x++)
            {
                sudokuTable[x + 9 * y].GetComponent<Cell>().SetGridPos(x, y);
            }
        }
    }

    public void CellsToSudokuTable()
    {
        for(int i = 0; i < 81; i++)
        {
            sudokuTable[i].GetComponent<Cell>().ParseCellStruct(cells[i]);
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
                sudokuTable[activeCellX + 9 * i].GetComponent<Cell>().SetCellActive(false);
                sudokuTable[i + activeCellY * 9].GetComponent<Cell>().SetCellActive(false);
            }
        }


        activeCellX = x;
        activeCellY = y;
        for (int i = 0; i < 9; i++)
        {
            sudokuTable[activeCellX + 9 * i].GetComponent<Cell>().SetCellActive(true);
            sudokuTable[i + activeCellY * 9].GetComponent<Cell>().SetCellActive(true);
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
            int isRightNumber = sudokuTable[activeCellX + activeCellY * 9].GetComponent<Cell>().SetUserNumber(value);
            if (isRightNumber == -1)
                ManageMistakesCount();
            ChangeRightNumberCount(isRightNumber);
        }
    }

    //Set note = value
    public void SetNoteInCell(int value)
    {
        //if there is active cell, set note
        if (activeCellX != -1)
        {
            sudokuTable[activeCellX + activeCellY * 9].GetComponent<Cell>().SetLittleNumber(value);
        }
    }

    //clear cells with flag isEmpty == true
    public void RestartGame()
    {
        int idx = 0;
        foreach (GameObject cell in sudokuTable)
        {
            if (cell.GetComponent<Cell>().GetIsEmpty)
            {
                //clear notes and number in cell
                cell.GetComponent<Cell>().ClearCellInRestart();

                //reset cell buffer
                cells[idx] = cell.GetComponent<Cell>().cellData;
            }
            idx++;
        }

        //reset lives
        lives.increaseLives(3);
        hintCount = 3;
    }

    //fill active cell with right number of hint color
    //SetActive(false), if hint count is bigger than possible(to make hints unavailable)
    public void MakeAHint()
    {
        if (activeCellX != -1)
        {
            Cell tempCell = sudokuTable[activeCellX + activeCellY * 9].GetComponent<Cell>();
            if (tempCell.GetIsEmpty & tempCell.ManageUserValue == 0)
            {
                //fill cell with main number of hint color
                sudokuTable[activeCellX + activeCellY * 9].GetComponent<Cell>().SetHint();
                ChangeRightNumberCount(1);
                hintCount--;                //decrease available hint count
                if(hintCount <= 0)          //if there are no available hints
                    GameObject.FindGameObjectWithTag("HintButton").SetActive(false);
            }
               
        }
    }

    //Set cell empty
    public void ClearCell()
    {
        sudokuTable[activeCellX + activeCellY * 9].GetComponent<Cell>().ClearCell();
    }
    ///////////////////////////////
}
