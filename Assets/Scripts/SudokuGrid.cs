using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;

[System.Serializable]
public struct SaveDataStruct
{
    [SerializeField] public ModesStruct modes;      //choosen modes
    [SerializeField] public CellStruct[] cells;     //numbers and notes in cells
    [SerializeField] public float timerValue;       //time spent on current sudoku
    [SerializeField] public int[] countOfDifNums;   //count of all types of nums

    //
    [SerializeField] public int countRightNumbers;
    [SerializeField] public int livesLeft;
    [SerializeField] public int hintCount;
}

public class SudokuGrid : MonoBehaviour
{
    public HeartConroller lives;            // lives manager
    private bool isSetModes;

    public GameObject WinPanel;             // Panel, that will apear after win
    public GameObject GameOverPanel;        // Panel, that will apear after lose
    public GameObject Timer;                // timer object
    public GameObject LivesObj;             // lives objects
    public GameObject HintObj;
    public List<GameObject> NumButtons;

    private bool isEnterBigNumbers;         // enter mode: is user enter notes or big numbers in cell (false for notes)

    public int[] countOfDifNums;            //count of all types of nums
    private int countRightNumbers;          // count number of right filled cells, if equal to 81, game is won
    private int hintCount;                  // number of available hints
    private int activeCellX, activeCellY;   // position of hightlighted cell (suducku table positions: from 0 to 80)

    private float saveTimer;
    private float savePeriod;

    //modes
    [SerializeField] private SaveDataStruct SudokuData;

    //save data
    [SerializeField] private List<GameObject> sudokuTable;   // list of cells as game objects
    [SerializeField] private CellStruct[] cells;  //buffer of cells
    [SerializeField] private string saveFilePath; //saved data path

    private void Awake()
    {
        //saved data path
        saveFilePath = Application.persistentDataPath + "/Save.dat";

        //load saved data, if continue pressed
        if (DataHolder.IsContinueMode)
        {
            SudokuData = SaveSystem.LoadData<SaveDataStruct>(saveFilePath);
            cells = SudokuData.cells;
            Timer.GetComponent<TimerScript>().ManageTimerValue = SudokuData.timerValue;
            countOfDifNums = SudokuData.countOfDifNums;
            countRightNumbers = SudokuData.countRightNumbers;
            hintCount = SudokuData.hintCount;

            CellsToSudokuTable();
            File.Delete(saveFilePath);
        }
    }

    private void Start()
    {
        savePeriod = 10000;

        isSetModes = false;
        saveTimer = 0;

        //if continue mode
        if (DataHolder.IsContinueMode)
        {
            ////GetGridPrefs();
            lives.CheckIsLivesRight(SudokuData.livesLeft);

            DataHolder.ManageModes = SudokuData.modes;
        }
        else
        {
            //clear prefs needed to grid
            ////ClearGridPrefs();

            InitCountOfDifNums();
            SetRightNumberCount();
            hintCount = 3;

            //set modes chosen
            SudokuData.modes = DataHolder.ManageModes;

            //save data buffer
            cells = new CellStruct[81];
            SetIndexes();
        }

        //Set chosen modes
        SetModes();

        //if none of cells is highlited
        activeCellX = -1;
        activeCellY = -1;


        //isGenerated = false;
        isEnterBigNumbers = true;
    }

    void ClearGridPrefs()
    {
        PlayerPrefs.DeleteKey("countRightNumbers");
        PlayerPrefs.DeleteKey("hintCount");
        PlayerPrefs.DeleteKey("livesLeft");
        PlayerPrefs.Save();
    }

    void SetGridPrefs()
    {
        PlayerPrefs.SetInt("countRightNumbers", countRightNumbers);
        PlayerPrefs.SetInt("livesLeft", lives.GetLives());
        PlayerPrefs.SetInt("hintCount", hintCount);
        PlayerPrefs.Save();
    }

    void GetGridPrefs()
    {
        countRightNumbers = PlayerPrefs.GetInt("countRightNumbers");
        hintCount = PlayerPrefs.GetInt("hintCount");
        lives.CheckIsLivesRight(PlayerPrefs.GetInt("livesLeft"));
    }

    private void OnApplicationQuit()
    {
        SaveCurCells();
    }

    private void Update()
    {
        //save sudoku every 10 seconds
        saveTimer += Time.deltaTime;
        if (saveTimer >= savePeriod)
        {
            SaveCurCells();
            saveTimer = 0;
        }

        //if need to change modes
        if(isSetModes)
        {
            //bacause this construction works when game is new
            SetRightNumberCount();

            SetModes();
            isSetModes = false;
            lives.CheckIsLivesRight();
        }
    }

    //init count of right numbers
    void SetRightNumberCount()
    {
        countRightNumbers = 81 - DataHolder.ManageDifficulty;
    }

    void InitCountOfDifNums()
    {
        countOfDifNums = new int[9];
        for (int i = 0; i < 9; i++)
            countOfDifNums[i] = 0;
    }

    public void ChangeCountOfDifNum(int number, int changeNum)
    {
        countOfDifNums[number-1] += changeNum;
        CheckCountOfDifNums(number);
    }

    //number of amount of each number
    void CheckCountOfDifNums(int number)
    {
        Debug.Log(countOfDifNums[number - 1]);
        if (countOfDifNums[number-1] == 9)
        {
            NumButtons[number - 1].GetComponent<Image>().color = Color.green;
        }
        else if(!NumButtons[number - 1].activeInHierarchy)
        {
            Debug.Log("IT'S WAS INACTIVE!!!");
            NumButtons[number - 1].GetComponent<Image>().color = Color.black;
        }
    }


    void CheckEveryFlagAfterEnter(int number, int wasItRight, bool isItRight, bool isNote = false)
    {
        if(wasItRight == -1) //so it was right
        {
            //decrease this num count and right number count
            ChangeCountOfDifNum(number, -1);
            ChangeRightNumberCount(-1);
        }
        else if(isItRight)
        {
            //increase this num count
            ChangeCountOfDifNum(number, 1);
            ChangeRightNumberCount(1);
        }
        else 
        {
            //so it's mistake
            if (!isNote)
                ManageMistakesCount();
        }
        
    }

    //broadcast that there is a need to change modes
    public void SetModeChangeFlag()
    {
        isSetModes = true;
        SudokuData.modes = DataHolder.ManageModes;
    }

    //change modes to chosen ones
    private void SetModes()
    {
        HintObj.SetActive(hintCount > 0 && SudokuData.modes.isHints);
        Timer.SetActive(SudokuData.modes.isTimer);
        LivesObj.SetActive(SudokuData.modes.isLives && SudokuData.modes.isMistakes);
    }

    public void SaveCurCells()
    {
        ////SetGridPrefs();

        for (int i = 0; i < 81; i++)
        {
            cells[i] = sudokuTable[i].GetComponent<Cell>().cellData;
        }
        SudokuData.cells = cells;
        SudokuData.timerValue = Timer.GetComponent<TimerScript>().ManageTimerValue;
        SudokuData.countOfDifNums = countOfDifNums;

        SudokuData.hintCount = hintCount;
        SudokuData.countRightNumbers = countRightNumbers;
        SudokuData.livesLeft = lives.GetLives();

        SaveSystem.SaveData(saveFilePath, SudokuData);
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
        Debug.Log(_isNumberRight + "  countRightNumbers = " + countRightNumbers);
        countRightNumbers += _isNumberRight;
        Debug.Log("  After countRightNumbers = " + countRightNumbers);
        if (countRightNumbers == 81)
        {
            WinPanel.SetActive(true);
            return;
        }
    }

    //Increase mistakes and change one heart to gray
    public void ManageMistakesCount()
    {
        if(SudokuData.modes.isMistakes && SudokuData.modes.isLives)
            if (lives.decreaseLives())
                GameOverPanel.SetActive(true);
    }

    //Highlight cell
    public void SetActiveCell(int x, int y)
    {
        ClearActiveCells();
        activeCellX = x;
        activeCellY = y;
        ShowAllChoseNumbers();
        for (int i = 0; i < 9; i++)
        {
            sudokuTable[activeCellX + 9 * i].GetComponent<Cell>().SetCellActive(true);
            sudokuTable[i + activeCellY * 9].GetComponent<Cell>().SetCellActive(true);
        }
    }

    public void ClearActiveCells()
    {
        //clear active cells
        if (activeCellX != -1)
        {
            for (int i = 0; i < 9; i++)
            {
                sudokuTable[activeCellX + 9 * i].GetComponent<Cell>().SetCellActive(false);
                sudokuTable[i + activeCellY * 9].GetComponent<Cell>().SetCellActive(false);
            }
        }
    }

    public void ShowAllChoseNumbers()
    {
        int number;
        if (sudokuTable[activeCellX + activeCellY * 9].GetComponent<Cell>().GetIsEmpty)
            number = sudokuTable[activeCellX + activeCellY * 9].GetComponent<Cell>().ManageUserValue;
        else
            number = sudokuTable[activeCellX + activeCellY * 9].GetComponent<Cell>().GetMainValue;

        if (number == 0)
            return;

        foreach (GameObject cell in sudokuTable)
        {
            cell.GetComponent<Cell>().SetHightLighted(number);
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
    public void SetNumberInCell(int number)
    {
        if(activeCellX != -1)
        {
            int isRightNumber = sudokuTable[activeCellX + activeCellY * 9].GetComponent<Cell>().SetUserNumber(number);
            
            if (isRightNumber == -1)
            {
                ManageMistakesCount();
            }

            if (isRightNumber == 1)
            {
                ClearNotesOfRightNumber(number);
                ChangeCountOfDifNum(number, 1);
                ChangeRightNumberCount(1);
            }

            if (isRightNumber == 0)
            {
                CheckEveryFlagAfterEnter(number, -1, false);
            }
        }
    }

    //Set note = value
    public void SetNoteInCell(int value)
    {
        //if there is active cell, set note
        if (activeCellX != -1)
        {
            int[] values = sudokuTable[activeCellX + activeCellY * 9].GetComponent<Cell>().SetLittleNumber(value);
            //ChangeRightNumberCount(wasUserNumRight);
            CheckEveryFlagAfterEnter(values[0], values[1], false, true);
        }
    }

    //if was entered right number,
    //clear notes of this number
    //in the row, column and block
    void ClearNotesOfRightNumber(int number)
    {
        for (int i = 0; i < 9; i++)
        {
            //clear notes with user value == number in row
            sudokuTable[i + activeCellY * 9].GetComponent<Cell>().CheckConsistOfNoteNumber(number, true);

            //clear notes with user value == number in column
            sudokuTable[activeCellX + i * 9].GetComponent<Cell>().CheckConsistOfNoteNumber(number, true);
        }

        //clear notes with user value == number in block
        int idx = activeCellX + activeCellY * 9;
        int blk = GetBlockNumber(idx);

        int startPos = (idx / 27) * 27 + blk%3*3;

        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                sudokuTable[startPos + i * 9 + j].GetComponent<Cell>().CheckConsistOfNoteNumber(number, true);
                int pos = startPos + i * 9 + j;
            }
        }   
    }

    private int GetBlockNumber(int idx)
    {
        //redo it/////
        int row = idx / 27;
        int column = idx%9/3;
        return row*3 + column;
        //redo it/////
    }

    //clear cells with flag isEmpty == true
    public void RestartGame()
    {
        int idx = 0;
        SetRightNumberCount();
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

        SetModes();
    }

    //fill active cell with right number of hint color
    //SetActive(false), if hint count is bigger than possible(to make hints unavailable)
    public void MakeAHint()
    {
        if (activeCellX != -1)
        {
            Cell tempCell = sudokuTable[activeCellX + activeCellY * 9].GetComponent<Cell>();
            if (tempCell.GetIsEmpty/* & tempCell.MatchUserAndMainValues()*/)
            {
                //fill cell with main number of hint color
                sudokuTable[activeCellX + activeCellY * 9].GetComponent<Cell>().SetHint();
                ChangeRightNumberCount(1);
                hintCount--;                //decrease available hint count
                if (hintCount <= 0)          //if there are no available hints
                    GameObject.FindGameObjectWithTag("HintButton").SetActive(false);
                ClearNotesOfRightNumber(sudokuTable[activeCellX + activeCellY * 9].GetComponent<Cell>().GetMainValue);
            }

        } 
    }

    //Set cell empty
    public void ClearCell()
    {
        int[] values = sudokuTable[activeCellX + activeCellY * 9].GetComponent<Cell>().ClearCell();
        CheckEveryFlagAfterEnter(values[0], values[1], false);
    }
    ///////////////////////////////
}
