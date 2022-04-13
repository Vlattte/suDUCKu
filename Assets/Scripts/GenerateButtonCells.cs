using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateButtonCells : MonoBehaviour
{
    private float xPos, yPos;

    public GameObject gridParent;
    public GameObject cellButtonPrefab;
    public GameObject cellClearerPrefab;
    public GameObject noteBtnCellPrefab;
    public SuduckuGrid grid;

    private List<GameObject> buttons;
    private bool isButtonsSpawned;

    private void Start()
    {
        buttons = new List<GameObject>();
        isButtonsSpawned = false;
        grid = gridParent.GetComponent<SuduckuGrid>();
        xPos = grid.firstX;
        yPos = grid.lastY;
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
        xPos -= 9 * grid.cellRadius;
        yPos -= 12 * grid.cellRadius;

        for (int i = 0; i < 9; i++)
        {
            Vector2 curCellPos = new Vector2(xPos + i * grid.cellRadius * 2 + grid.cellRadius, yPos);

            var newCell = Instantiate(cellButtonPrefab, curCellPos, Quaternion.identity);
            ButtonCell cellRef = newCell.GetComponent<ButtonCell>();

            buttons.Add(newCell as GameObject);
            cellRef.SetButtonValue = i+1;

            newCell.transform.SetParent(this.transform);
        }
        
        //create clear button
        var clearCell = Instantiate(cellClearerPrefab, new Vector2(xPos + grid.cellRadius, yPos - 2*grid.cellRadius), Quaternion.identity);
        clearCell.transform.SetParent(this.transform);

        //create note button
        var noteBtnCell = Instantiate(noteBtnCellPrefab, new Vector2(xPos + 4*grid.cellRadius, yPos - 2 * grid.cellRadius), Quaternion.identity);
        noteBtnCell.transform.SetParent(this.transform);
    }
}
