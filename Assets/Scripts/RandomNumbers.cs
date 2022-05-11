using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RandomNumbers: MonoBehaviour
{
    //cell positions
    public float gridSizeX, gridSizeY;

    public List<GameObject> suduckuTable;
    public List<GameObject> buttons;        //buttons, which enter numbers in cells
    public GameObject cellPrefab;
    public SudokuGenerator generator;
    public HeartConroller lives;

    public bool isGenerated = false;                // bool for EnterCells to begin spawning buttons

    private void Start()
    {
        if (!isGenerated)
        {
            SetIndexes();
            isGenerated = true;
        }
            
           
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
}
