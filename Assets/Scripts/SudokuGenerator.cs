using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SudokuGenerator : MonoBehaviour
{
    public List<GameObject> SuduckuTable;
    private bool isGenerated;
    private Cell[,] cells;


    private void Update()
    {
        if (SuduckuTable != null && !isGenerated)
        {
            isGenerated = true;
            GenerateBaseTable();
            RandomMixing();

            SudokuSolver.Writeln(CellsToNumbers(), true);
            ErraisingNumbers(47);

            isGenerated = SudokuSolver.MainFunction(CellsToNumbers());
            if (!isGenerated)
                GenerateAgain();
        }
    }

    public void GenerateAgain()
    {
        SuduckuTable.RemoveRange(0, SuduckuTable.Count);
        isGenerated = false;
    }

    void Start()
    {
        cells = new Cell[9,9];
        isGenerated = false;
    }

    void GenerateBaseTable()
    {
        GameObject temp = SuduckuTable[0];
        SuduckuTable.Remove(temp);
        SuduckuTable.Insert(SuduckuTable.Count, temp);

        int[] nums = { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
        int k = -1;

        for (int i = 0; i < 9; i++)
        {
            if (i% 3 == 0)
                k++; 
            for (int j = 0; j < 9; j++)
            {
                Cell cellRef = SuduckuTable[i*9+j].GetComponent<Cell>();
                cells[j, i] = cellRef;
                cellRef.SetNumber(nums[(j + (i*3) + k)%9]);
            }
        }
    }


    void RandomMixing()
    {
        // 1 - transposing; 2 - SwapRowInSector; 3 - SwapColInSector
        // 4 - SwapRowSectors; 5 - SwapColSectors

        for (int i = 0; i < 10; i++)
        {
            switch(Random.Range(1, 6))
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

    void ErraisingNumbers(int difficulty)
    {
        while (difficulty > 0)
        {
            int x = Random.Range(0, 9);
            int y = Random.Range(0, 9);
            if (cells[x, y].isEmpty)
                continue;

            cells[x, y].SetNumber(0);
            difficulty--;
        }
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


                temp = cells[i, k].GetMainValue;
                cells[i, k].SetNumber(cells[k, i].GetMainValue);
                cells[k, i].SetNumber(temp);
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
            buf = cells[cur_col, row1].GetMainValue;
            cells[cur_col, row1].SetNumber(cells[cur_col, row2].GetMainValue);
            cells[cur_col, row2].SetNumber(buf);
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
            buf = cells[col1, cur_row].GetMainValue;
            cells[col1, cur_row].SetNumber(cells[col2, cur_row].GetMainValue);
            cells[col2, cur_row].SetNumber(buf);
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

    int[] CellsToNumbers()
    {
        int[] cell_numbers = new int[81];
        int i = 0;
        for (int x = 0; x < 9; x++)
        {
            for (int y = 0; y < 9; y++)
            {
                cell_numbers[i] = cells[y, x].GetMainValue;
                i++;
            }
        }
        return cell_numbers;
    }
}
