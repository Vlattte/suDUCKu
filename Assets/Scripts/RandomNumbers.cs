using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RandomNumbers: MonoBehaviour
{
    //cell positions
    public float gridSizeX, gridSizeY;

    public List<GameObject> SuduckuTable;

    private bool isGenerated = false;                // bool for EnterCells to begin spawning buttons
    private float timer_for_next_generation;

    private int[,] cellNumbers;

    private void Start()
    {
        cellNumbers = new int[9, 9];
        timer_for_next_generation = 0f;
        SetIndexes();
        isGenerated = false;
    }

    void SetIndexes()
    {
        for (int y = 0; y < 9; y++)
        {
            for (int x = 0; x < 9; x++)
            {
                SuduckuTable[x + 9 * y].GetComponent<Cell>().SetGridPos(x, y);
            }
        }
    }

    private void Update()
    {
        if (!isGenerated)
        {
            isGenerated = true;
            GenerateBaseTable();        //generate base table
            RandomMixing();             //mix values
            ErraisingNumbers(47);       //clear some cells           
        }

        timer_for_next_generation += Time.deltaTime;
        Debug.Log(timer_for_next_generation);
        if(timer_for_next_generation > 5)
        {
            isGenerated = false;
            timer_for_next_generation = 0f;
        }
    }

    void FillCellsAfterMixing()
    {
        for (int x = 0; x < 9; x++)
        {
            for (int y = 0; y < 9; y++)
            {
                SuduckuTable[x * 9 + y].GetComponent<Cell>().SetNumberUI(cellNumbers[x, y]);
            }
        }
    }

    void GenerateBaseTable()
    {
        int[] nums = { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
        int k = -1;

        for (int i = 0; i < 9; i++)
        {
            if (i % 3 == 0)
                k++;
            for (int j = 0; j < 9; j++)
            {
                //Cell cellRef = SuduckuTable[i*9+j].GetComponent<Cell>();
                //cells[j, i] = cellRef;
                //cellRef.RememberNumber(nums[(j + (i*3) + k)%9]);
                cellNumbers[i, j] = nums[(j + (i * 3) + k) % 9];
            }
        }
    }

    //GENERATING RANDOM SUDOKU

    void RandomMixing()
    {
        // 1 - transposing; 2 - SwapRowInSector; 3 - SwapColInSector
        // 4 - SwapRowSectors; 5 - SwapColSectors

        for (int i = 0; i < 10; i++)
        {
            switch (Random.Range(1, 6))
            {
                case 1:
                    Transposing();
                    break;
                case 2:
                    SwapRowInSector();
                    break;
                case 3:
                    SwapColInSector();
                    break;
                case 4:
                    SwapRowSectors();
                    break;
                case 5:
                    SwapColSectors();
                    break;
            }
        }
    }

    //make cells empty, depends on difficulty level
    void ErraisingNumbers(int difficulty)
    {
        while (difficulty > 0)
        {
            int x = Random.Range(0, 9);
            int y = Random.Range(0, 9);

            if (cellNumbers[x, y] < 0)
            {
                //if (SuduckuTable[x * 9 + y].GetComponent<Cell>().isEmpty)
                continue;
            }

            cellNumbers[x, y] = -cellNumbers[x, y];
            difficulty--;
        }
        FillCellsAfterMixing();
    }

    void Transposing()
    {
        for (int i = 0; i < 9; i++)
        {
            for (int k = 0; k < i; k++)
            {
                int temp;
                if (i == k)
                    continue;

                temp = cellNumbers[i, k];
                cellNumbers[i, k] = cellNumbers[k, i];
                cellNumbers[k, i] = temp;
            }
        }
    }

    void SwapRowInSector(int sector = 0, int sector2 = -1, int row1 = 0, int row2 = 0)
    {
        if (sector2 == -1)
        {
            sector = Random.Range(0, 3);
            row1 = Random.Range(0, 3) + sector * 3;
            row2 = Random.Range(0, 3) + sector * 3;
            if (row1 == row2)
            {
                if (row1 % 3 == 0)
                    row1 += 1;
                else
                    row1 -= 1;
            }
        }
        else
        {
            row1 += sector * 3;
            row2 += sector2 * 3;
        }


        int buf;
        for (int cur_col = 0; cur_col < 9; cur_col++)
        {
            buf = cellNumbers[cur_col, row1];
            cellNumbers[cur_col, row1] = cellNumbers[cur_col, row2];
            cellNumbers[cur_col, row2] = buf;
        }
    }

    void SwapColInSector(int sector = 0, int sector2 = -1, int col1 = 0, int col2 = 0)
    {
        if (sector2 == -1)
        {
            sector = Random.Range(0, 3);
            col1 = Random.Range(0, 3) + sector * 3;
            col2 = Random.Range(0, 3) + sector * 3;
            if (col1 == col2)
            {
                if (col1 % 3 == 0)
                    col1 += 1;
                else
                    col1 -= 1;
            }
        }
        else
        {
            col1 += sector * 3;
            col2 += sector2 * 3;
        }

        int buf;
        for (int cur_row = 0; cur_row < 9; cur_row++)
        {
            buf = cellNumbers[col1, cur_row];
            cellNumbers[col1, cur_row] = cellNumbers[col2, cur_row];
            cellNumbers[col2, cur_row] = buf;
        }
    }

    void SwapRowSectors()
    {
        int sector1 = Random.Range(0, 3);
        int sector2 = Random.Range(0, 3);

        if (sector1 == sector2)
        {
            if (sector1 == 2)
                sector1 -= 1;
            else
                sector1 += 1;
        }

        for (int row = 0; row < 3; row++)
        {
            SwapRowInSector(sector1, sector2, row, row);
        }
    }

    void SwapColSectors()
    {
        int sector1 = Random.Range(0, 3);
        int sector2 = Random.Range(0, 3);

        if (sector1 == sector2)
        {
            if (sector1 == 2)
                sector1 -= 1;
            else
                sector1 += 1;
        }

        for (int col = 0; col < 3; col++)
        {
            SwapColInSector(sector1, sector2, col, col);
        }
    }

    //////////////////////////
}
