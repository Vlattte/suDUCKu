using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;

public class SudokuSolver
{
    static bool isSudokuCreated;

    public static bool MainFunction(int[] sudoku)
    {
        int index = 0;
        /*sudoku = new int[81] {0,0,0,  1,0,6,  2,0,0,  // ������ ������
                              8,0,2,  0,0,5,  0,0,0,  // ������ - ���� ������
                              0,9,4,  0,0,2,  0,0,0,  // ������ - 9 �������� �� �����������
                                                    // ������� - 9 �������� �� ���������
                              2,0,0,  3,0,0,  0,0,1,  // ������� - ������ �� 9 �������� 
                              0,0,0,  0,2,9,  7,0,0,
                              4,0,0,  0,6,1,  0,8,2,

                              9,4,0,  2,5,7,  0,0,0,
                              0,0,5,  9,0,0,  0,2,7,
                              0,2,0,  6,1,0,  5,9,4};*/

        //����� ������ �� �����
        Debug.Log("������");
        Writeln(sudoku, false);
        if (CheckSudoku(sudoku)) //������� ��������� � ������ ������������ ������
        {
            //���� ������ ����������, �� ������ �������
            if (Solver(index, ref sudoku))  //������� ��������� ���� �� �������,                                                 
            {                               //���� ����, ����� ���������� true � �� ref ������ ������ ��������� �������
                Debug.Log("������ ��������� ������� ������");
                Writeln(sudoku, false);
            }
            else Debug.Log("��� �������");

        }
        else
        {
            Debug.Log("������ ��������� � ��������");
            return false;
        }
        return true;
    }


    //����� �������� ������ �� ������������ �����������
    static bool CheckSudoku(int[] sudoku)               // ����� ���������� true, ���� ������ ��������� ���������
    {
        for (int index = 0; index < 81; index++)
        {
            if (9 < sudoku[index] || sudoku[index] < 0) //�������� �������� ����� � ��������� 1 �� 9
                return false;

            if (sudoku[index] != 0)                     //��������� �� ���������� ������ ��������� ������
            {
                //��������� ������ �� ���������� �������� � ������� �������� � ������, �������, ��������
                if (CheckPossibleValue(sudoku[index], index, sudoku) == false)
                    return false;
            }
        }
        return true;
    }

    //����� ������ ������� ������
    static bool Solver(int index, ref int[] sudoku)
    {
        if ((index < 81) && (sudoku[index] == 0))   //��������� ������ ������, ����� ����������� � ��������� ����� �� ������� ������� ������
        {
            Stack<int> stackPossibleValues = new Stack<int>();                  //�������������� ���� ��� ��������� �������� ������
            for (int possibleValue = 1; possibleValue < 10; possibleValue++)    //��������������� ��������� �������� �� 1 �� 9 � ���������� � ����
            {
                if (CheckPossibleValue(possibleValue, index, sudoku))           //���� ��� ���������� � ������� ������ � ��������
                    stackPossibleValues.Push(possibleValue);                    //���������� �������� � ����
            }

            while (stackPossibleValues.Count > 0)           //��������� �������� �� ����� ��� ������, ���� ���� �� ��������
            {
                sudoku[index] = stackPossibleValues.Pop();  //����������� ������ �������� �� �����
                if (Solver(index + 1, ref sudoku))          //����� �������� ��� ���� � ����������� ������� ������� ������
                    return true;                            //������� if ������ true, ������ ���� �������� ����� ������, �.�. ��� ������ ���������
            }
            sudoku[index] = 0;      //���� ��� �������� ����� ���������, � �� ��������� ������� ������������ false, 
            return false;           //�� �������� ������ � ������� index, ���������� ����������� ������ false

        }
        else if (index < 81)        //����������� ������ ����������. 
        {
            if (Solver(index + 1, ref sudoku) == false)
                return false;
        }
        return true;                //������� true ����� ��� index = 81, �.�. ��� ������ ���������, 
                                    //���������� ���������������� ����� �� ���� ��������� �������, �.�. ��� ������ ����� ���������� true
    }

    //����� ��������� �������� �� ���������� � ������� �������� � ������, �������, ��������
    //� ����� ���������: ����������� ��������, ������ ������, ��� ����������� ��� ��������, � ������ ������
    static bool CheckPossibleValue(int value, int index, int[] sudoku)
    {
        //��������� �� ������ � �������
        for (int j = 0; j < 9; j++)
        {
            if (value == sudoku[(index / 9) * 9 + j]    //������� ������ ������� ������ (index / 9) * 9
                & index != (index / 9) * 9 + j)         //������� ����� ������ �� ���������� ���� � �����
            {
                return false;                           //���������� false, ���� ���� ���������� �������� � �������� � ������
            }

            if (value == sudoku[(index % 9) + 9 * j]    //������� ������ ������� ������� (index % 9) + 9
                & index != (index % 9) + 9 * j)
            {
                return false;                           //���������� false, ���� ���� ���������� �������� � �������� � �������
            }
        }
        //��������� �� ��������
        for (int j = 0; j < 3; j++)
        {
            for (int k = 0; k < 3; k++)
            {                                           //������� ������ ������� �������� ((index / 27 * 27) + (index % 9) / 3 * 3)
                if (value == sudoku[(index / 27 * 27) + ((index % 9) / 3 * 3) + (9 * j) + k]
                    & index != ((index / 27 * 27) + (index % 9) / 3 * 3 + (9 * j) + k))
                {
                    return false;                       //���������� false, ���� ���� ���������� �������� � �������� � ��������
                }
            }
        }
        return true;                                    //���������� true, ���� ���������� �� ����
    }

    //������ ����� ������ �� ����� ������ � ������������ ����
    public static void Writeln(int[] sudoku, bool isNotErrasedSudoku)
    {
        StreamWriter sw;
        if (isNotErrasedSudoku)
        {
            sw = new StreamWriter("D:/unity/suducku/Assets/Scripts/text.txt", false);
            sw.Write("BEFORE ERRAISING\n");
        }
        else
            sw = new StreamWriter("D:/unity/suducku/Assets/Scripts/text.txt", true);


        sw.Write(new string('-', 21));
        sw.Write("\n");
        for (int i = 0; i < 81; i++)
        {
            sw.Write("{0} ", sudoku[i]);
            if (((i + 1) % 3 == 0) & ((i + 1) % 9 != 0))
                sw.Write("| ");

            if ((i + 1) % 9 == 0)
                sw.WriteLine();

            if ((i + 1) % 27 == 0)
                sw.WriteLine(new string('-', 21));
        }
        if (isNotErrasedSudoku)
            sw.Write("BEFORE ERRAISING\n");
        sw.Write("\n\n\n\n");
        sw.Close();
    }
}

