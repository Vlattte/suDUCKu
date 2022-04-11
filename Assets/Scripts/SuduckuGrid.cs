using System.Collections.Generic;
using UnityEngine;


public class SuduckuGrid : MonoBehaviour
{
    public float gridSizeX, gridSizeY;
    public float cellRadius;
    private float cellDiameter;
    private int cellsCountX, cellsCountY;
    public float firstX, lastY;

    public List<GameObject> suduckuTable;
    public List<GameObject> buttons;
    public GameObject cellObject;

    public SudokuGenerator generator;
    public bool isGenerated;            // bool for EnterCells to begin spawning buttons

    private void Start()
    {
        isGenerated = false;
        cellDiameter = gridSizeX/9;
        cellRadius = cellDiameter/2;
        cellsCountX = Mathf.RoundToInt(gridSizeX / cellDiameter);
        cellsCountY = Mathf.RoundToInt(gridSizeY / cellDiameter);

        suduckuTable = new List<GameObject>();

        CreateGrid();
        isGenerated = true;
    }

    void CreateGrid()
    {
        Vector3 gridLeftUpperCorner = transform.position - Vector3.right * gridSizeX / 2 + Vector3.up * gridSizeY / 2;

        for (int y = 0; y < 9; y++)
        {
            for (int x = 0; x < 9; x++)
            {
                Vector3 curCellPos = gridLeftUpperCorner + (x * cellDiameter + cellRadius) * Vector3.right
                                                         + (y * cellDiameter + cellRadius) * Vector3.down;
                if (x == 0 && y == 0)
                    firstX = curCellPos.x;
                if (x == 8 && y == 8)
                    lastY = curCellPos.y;

                Cell cellRef = cellObject.GetComponent<Cell>();

                //remember this cell in the list
                //make it child of an object
                suduckuTable.Add(Instantiate(cellObject) as GameObject);
                cellRef.SetGridPos(x, y);
                cellRef.ManageWorldPos = curCellPos;

                //Debug.Log("Cell " + cellRef.worldPos + "   =   " + cellRef.gridPosX + ":" + cellRef.gridPosY);

                suduckuTable[suduckuTable.Count - 1].transform.SetParent(this.transform);
                cellObject.transform.position = new Vector3(curCellPos.x, curCellPos.y, 0); //.position.Set(curCellPos.x, curCellPos.y, 0);
            }
        }

       /* Debug.Log(suduckuTable.Count - 1);
        Debug.Log(suduckuTable[suduckuTable.Count - 1].GetComponent<Cell>().gridPosX);
        Debug.Log(suduckuTable[suduckuTable.Count - 1].GetComponent<Cell>().gridPosY);*/
        generator.SuduckuTable = suduckuTable;
    }
}
