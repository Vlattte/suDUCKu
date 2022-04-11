using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterCells : MonoBehaviour
{
    private float xPos, yPos;

    public GameObject gridParent;
    public GameObject cellObject;
    public SuduckuGrid grid;

    private List<GameObject> cells;
    private bool isButtonsSpawned;

    private void Start()
    {
        cells = new List<GameObject>();
        isButtonsSpawned = false;
        grid = gridParent.GetComponent<SuduckuGrid>();
        /*xPos = grid.firstX;
        yPos = grid.lastY;*/
    }

    private void Update()
    {
        if (grid.isGenerated && !isButtonsSpawned)
        {
            isButtonsSpawned = true;
            CreateCells();
        }
    }

    void CreateCells()
    {
        Debug.Log("BEGUN");
        xPos -= 9 * grid.cellRadius;
        yPos -= 12 * grid.cellRadius;

        for (int i = 0; i < 9; i++)
        {
            Cell cellRef = cellObject.GetComponent<Cell>();
            cells.Add(Instantiate(cellObject) as GameObject);
            Vector2 curCellPos = new Vector3(xPos + i * grid.cellRadius * 2 + grid.cellRadius, yPos);
            cellRef.ManageWorldPos = curCellPos;

            cells[cells.Count - 1].transform.SetParent(this.transform);
            cellObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(curCellPos.x, curCellPos.y);
        }
    }
}
