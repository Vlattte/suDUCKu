using System.Collections.Generic;
using UnityEngine;


public class SuduckuGrid : MonoBehaviour
{
    //cell positions
    public float gridSizeX, gridSizeY;
    public float cellRadius;
    private float cellDiameter;
    public float firstX, lastY;
    private float offsetX, offsetY;
    private float offsetConst;
    private Vector3 gridLeftCornerPos;

    public List<GameObject> suduckuTable;
    private Cell[,] grid;
    public List<GameObject> buttons;        //buttons, which enter numbers in cells
    public GameObject cellPrefab;
    public SudokuGenerator generator;
   
    public bool isGenerated;                // bool for EnterCells to begin spawning buttons
    public bool isEnterBigNumbers;

    private int countMistakes;
    private int activeCellX, activeCellY;

    private void Start()
    {
        //none of cells is highlited
        activeCellX = -1;
        activeCellY = -1;

        isGenerated = false;
        isEnterBigNumbers = true;
        cellDiameter = cellRadius * 2;

        gridLeftCornerPos = GameObject.FindGameObjectWithTag("fisrtCellLeftUpperCorner").transform.position;

        offsetConst = 0.3f;
        offsetX = 0;
        offsetY = 0;

        suduckuTable = new List<GameObject>();
        grid = new Cell[9, 9];

        CreateGrid();
        isGenerated = true;
    }

    void CreateGrid()
    {
        Debug.Log(gridLeftCornerPos);
        Vector3 leftUpperCorner = gridLeftCornerPos + Vector3.left * cellRadius + Vector3.up * cellRadius;
        for (int y = 0; y < 9; y++)
        {
            for (int x = 0; x < 9; x++)
            {
                if (x % 3 == 0 && x != 0)
                {
                    offsetX += 0.7f;
                    Debug.Log(offsetX);
                }

                if (y % 3 == 0 && x == 0 && y!=0)
                {
                    offsetY += 0.7f;
                }

                Vector3 curCellPos = leftUpperCorner + (x * cellDiameter + cellRadius + offsetX) * Vector3.right
                                                         + (y * cellDiameter + cellRadius + offsetY) * Vector3.down;

                if (x == 0 && y == 0)
                    firstX = curCellPos.x;
                if (x == 8 && y == 8)
                    lastY = curCellPos.y;
         
                //remember this cell in the list
                //make it child of an object
                var newCell = Instantiate(cellPrefab, new Vector3(curCellPos.x, curCellPos.y), Quaternion.identity);
                Cell cellRef = newCell.GetComponent<Cell>();
                cellRef.SetGridPos(x, y);
                cellRef.ManageWorldPos = curCellPos;

                newCell.transform.SetParent(this.transform);
                suduckuTable.Add(newCell as GameObject);
            }
            offsetX = 0;
        }
        generator.SuduckuTable = suduckuTable;
    }

    public int ManageMistakesCount
    {
        set 
        { 
            countMistakes++; 
            if(countMistakes >=3)
                Application.Quit();
        }
    }

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

    public void ChangeEnterMode()
    {
        isEnterBigNumbers = !isEnterBigNumbers;
    }

    public bool GetEnterModeStatus
    {
        get { return isEnterBigNumbers; }
    }

    //user change numbers functions
    public void SetNumberInCell(int value)
    {
        if(activeCellX != -1)
        {
            suduckuTable[activeCellX + activeCellY * 9].GetComponent<Cell>().SetUserNumber(value);
        }
    }

    public void SetLittleNumberInCell(int value)
    {
        if (activeCellX != -1 && activeCellY != -1)
        {
            suduckuTable[activeCellX + activeCellY * 9].GetComponent<Cell>().SetLittleNumber(value);
        }
    }

    public void ClearCell()
    {
        suduckuTable[activeCellX + activeCellY * 9].GetComponent<Cell>().ClearCell();
    }
    ///////////////////////////////
}
