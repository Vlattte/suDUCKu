using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Cell : MonoBehaviour
{
    public CellStruct cellData;    
    public NotesController notesController; //set number in note cells

    //number images
    public Sprite[] imageNumber;

    //UI
    public TextMeshProUGUI textNumber;
    public Image backImage;

    //Grid controller
    private SudokuGrid GridManager;

    //Image colors
    private Color notActiveColor;
    private Color activeColor;

    //text colors
    private Color mistakeColor;
    private Color notChangableNumberColor;
    private Color changableNumberColor;
    private Color hintColor;

    private void Awake()
    {
        notChangableNumberColor = new Color(99 / 255f, 99 / 255f, 99 / 255f);
        hintColor = new Color(0, 0, 1);
        changableNumberColor = new Color(0, 0, 0);

        activeColor = new Color(0.6556604f, 0.896965f, 1);
        notActiveColor = Color.white;
        
        mistakeColor = new Color(0.5960785f, 0, 0);
    }

    private void Start()
    {
        GridManager = GameObject.FindGameObjectWithTag("GridManager").GetComponent<SudokuGrid>();
        cellData.isMistakes = DataHolder.ManageModes.isMistakes; 
        if (cellData.isMistakes)
            MakeRightMistakeColor();

        if (!DataHolder.IsContinueMode)
        {
            cellData.noteValuesInCell = new bool[9];
            for (int i = 0; i < 9; i++)
                cellData.noteValuesInCell[i] = false;   
            cellData.isAnyNotes = 0;
            cellData.userValue = 0;
        }
    }

    private void Update()
    {
        if(DataHolder.isModeChanged == true)
        {
            if (DataHolder.ManageModes.isMistakes)
                cellData.isMistakes = true;
            else
                cellData.isMistakes = false;
        }
    }

    public void ParseCellStruct(CellStruct _data)
    {
        cellData = _data;
        ReturnSaveDataInCell();
    }

    public void MakeRightMistakeColor()
    {
        mistakeColor = new Color(0.5960785f, 0, 0);
    }

    public bool GetIsEmpty
    {
        get { return cellData.isEmpty; }
    }

    public int GetMainValue
    {
        get { return cellData.mainValue; }
    }

    public int ManageUserValue
    {
        get { return cellData.userValue; }
        set { cellData.userValue = value; }
    }

    public bool MatchUserAndMainValues()
    {
        return cellData.userValue == cellData.mainValue;
    }

    public void SetGridPos(int _gridPosX, int _gridPosY)
    {
        cellData.gridPosX = _gridPosX;
        cellData.gridPosY = _gridPosY;
    }

    //set number in ui text mesh pro object
    void ChangeTextNumber(string _number, Color _color)
    {
        textNumber.text = _number.ToString();
        textNumber.color = _color;
    }

    //returns data in cell from save binary file
    //set back uservalue, mainvalue, notes, text color(mistake, hint, not changeble or changable),
    public void ReturnSaveDataInCell()
    {
        if(cellData.isEmpty)
        {
            //if user value was in cell 
            if (cellData.userValue != 0)
            {
                if (cellData.userValue != cellData.mainValue && DataHolder.ManageModes.isMistakes)
                {
                    //if there was a mistake
                    ChangeTextNumber(cellData.userValue.ToString(), new Color(0.5960785f, 0, 0));
                }
                else
                {
                    //if user value was equal to main value
                    ChangeTextNumber(cellData.userValue.ToString(), new Color(0, 0, 0));
                }
            }
            else if(cellData.isAnyNotes > 0)
            {
                for(int i = 0; i < 9; i++)
                {
                    if(cellData.noteValuesInCell[i] == true)
                        notesController.SetNoteActive(true, i+1);
                }
            }

            //if hint => change text color to hint color
            if (cellData.isHintCell == true)
                ChangeTextNumber(cellData.mainValue.ToString(), new Color(0, 0, 1));
        }
        else 
        {
            //if cell was filled while generating
            ChangeTextNumber(cellData.mainValue.ToString(), new Color(99 / 255f, 99 / 255f, 99 / 255f));
        }
    }

    //function to init cells, before game
    //-number to empty cell and mainvalue = number
    //number to change main number to number
    public void SetNumberUI(int number)
    {
        if (number == 0)
        {
            ChangeTextNumber(" ", changableNumberColor);
            cellData.isEmpty = true;
        }
        else if (number < 0)
        {
            //method is used for Empty cell, when sudocku is creating
            //set empty cell, but remember this main value
            cellData.mainValue = -1 * number;
            cellData.isEmpty = true;

            ChangeTextNumber(" ", changableNumberColor);
        }
        else
        {
            if (cellData.isAnyNotes > 0)
                ClearNotes();

            //set cell with shown main number, so it is not empty
            cellData.mainValue = number;
            cellData.isEmpty = false;
            if(GridManager)
                GridManager.ChangeCountOfDifNum(number, 1);

            ChangeTextNumber(cellData.mainValue.ToString(), notChangableNumberColor);
        }
    }

    //function to change number in the cell
    //number range [1, 9]
    //returns  1  if number == mainvalue
    //returns -1 if number != mainvalue
    //returns  0  if number == 0 or number == userValue
    public int SetUserNumber(int number)
    {
        if (cellData.isEmpty)
        {
            if (cellData.userValue == number)
            {
                //if(MatchUserAndMainValues())
                    ClearCell();
                    return 0;

            }
            else 
            {
                //if there is any user notes, 
                //clear them and write number
                if (cellData.isAnyNotes > 0)
                    ClearNotes();

                cellData.userValue = number;

                //if number isn't right increase mistakes
                //and set mistake color
                if (cellData.userValue != cellData.mainValue)
                {
                    if(cellData.isMistakes)
                        ChangeTextNumber(cellData.userValue.ToString(), mistakeColor);
                    else
                        ChangeTextNumber(cellData.userValue.ToString(), changableNumberColor);
                    return -1;
                }
                else
                {
                    ChangeTextNumber(cellData.userValue.ToString(), changableNumberColor);
                    return 1;
                }
            } 
        }
        return 2;
    }

    //number range [1, 9]
    public int[] SetLittleNumber(int number)
    {
        //returns 0 if cell was clear or mistake
        //returns -1 if there was right value in cell
        int[] returnValues = new int[2];
        returnValues[0] = cellData.mainValue;

        if (cellData.isEmpty)
        {
            //manage count of right numbers
            if (cellData.userValue != 0)
            {
                if (MatchUserAndMainValues())
                    returnValues[1] = -1;
                else
                    returnValues[1] = 0;    //mistake number in cell

                ClearCell();
            }
                

            if (cellData.noteValuesInCell[number - 1] == true)
            {
                //if this note is already in this cell, hide it 
                notesController.SetNoteActive(false, number);

                cellData.isAnyNotes--;
                cellData.noteValuesInCell[number - 1] = false;
            }
            else
            {
                //if there is no note in this cell, show it
                notesController.SetNoteActive(true, number);

                cellData.noteValuesInCell[number - 1] = true;
                cellData.isAnyNotes++;
            }
        }
        return returnValues;
    }

    public bool CheckConsistOfNoteNumber(int note, bool isClearAfterFinding)
    {

       if (cellData.noteValuesInCell[note-1])
       {
            if (isClearAfterFinding)
                SetLittleNumber(note);
            return true;
       }
       return false;
    }

    public void SetHint()
    {
        if (cellData.isAnyNotes > 0)
            ClearNotes();
        ChangeTextNumber(cellData.mainValue.ToString(), hintColor);

        cellData.userValue = cellData.mainValue;
        cellData.isHintCell = true;
    }

    private void OnMouseDown()
    {
        GridManager.SetActiveCell(cellData.gridPosX, cellData.gridPosY);
    }

    public void SetCellActive(bool isMakeActive)
    {
        if (isMakeActive)
            backImage.color = activeColor;
        else
            backImage.color = notActiveColor; 
    }

    //sets cell highlithed if number == number in cell
    public void SetHightLighted(int number)
    {
        if (cellData.userValue == number ||
           !cellData.isEmpty && cellData.mainValue == number)
        {
            backImage.color = activeColor;
        }
        else
        {
            backImage.color = notActiveColor;
        }
    }

    void ClearCellData()
    {
        cellData.userValue = 0;
        cellData.isAnyNotes = 0;
        cellData.isHintCell = false;

        for (int i = 0; i < 9; i++)
            cellData.noteValuesInCell[i] = false;

    }

    public void ClearCellInRestart()
    {
        ChangeTextNumber(" ", changableNumberColor);
        if (cellData.isAnyNotes > 0)
            ClearNotes();

        ClearCellData();
    }

    public int[] ClearCell()
    {
        int[] returnValues = new int[2];
        returnValues[0] = cellData.mainValue;
        returnValues[1] = 0;

        if (cellData.isEmpty)
        {
            //if GetEnterModeStatus == true
            //big numbers
            ChangeTextNumber(" ", activeColor);
            if (cellData.isAnyNotes > 0)
                ClearNotes();
            
            //check if player delete right number
            //and decrease right number count
            if (cellData.userValue == cellData.mainValue)
                returnValues[1] = -1;

            cellData.userValue = 0;
            cellData.isHintCell = false;
        }
        return returnValues;
    }

    private void ClearNotes()
    {
        for (int i = 0; i < 9; i++)
        {
            notesController.SetNoteActive(false, i+1);        
            cellData.noteValuesInCell[i] = false;
        }

        cellData.isAnyNotes = 0;
        cellData.isHintCell = false;
    }
}
