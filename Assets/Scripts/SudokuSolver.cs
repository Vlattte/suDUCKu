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
        /*sudoku = new int[81] {0,0,0,  1,0,6,  2,0,0,  // Вводим судоку
                              8,0,2,  0,0,5,  0,0,0,  // ячейка - один символ
                              0,9,4,  0,0,2,  0,0,0,  // строка - 9 символов по горизонтали
                                                    // столбец - 9 символов по вертикали
                              2,0,0,  3,0,0,  0,0,1,  // сегмент - группа из 9 символов 
                              0,0,0,  0,2,9,  7,0,0,
                              4,0,0,  0,6,1,  0,8,2,

                              9,4,0,  2,5,7,  0,0,0,
                              0,0,5,  9,0,0,  0,2,7,
                              0,2,0,  6,1,0,  5,9,4};*/

        //Вывод судоку на экран
        Debug.Log("Судоку");
        Writeln(sudoku, false);
        if (CheckSudoku(sudoku)) //Условие проверяет в методе правильность судоку
        {
            //Если судоку правильное, то ищется решение
            if (Solver(index, ref sudoku))  //Условие проверяет есть ли решение,                                                 
            {                               //если есть, метод возвращает true и по ref ссылке первое найденное решение
                Debug.Log("Первое найденное решение Судоку");
                Writeln(sudoku, false);
            }
            else Debug.Log("Нет решений");

        }
        else
        {
            Debug.Log("Судоку составлен с ошибками");
            return false;
        }
        return true;
    }


    //Метод проверки судоку на правильность составления
    static bool CheckSudoku(int[] sudoku)               // метод возвращает true, если судоку составлен правильно
    {
        for (int index = 0; index < 81; index++)
        {
            if (9 < sudoku[index] || sudoku[index] < 0) //Проверка значений ячеек в диапазоне 1 до 9
                return false;

            if (sudoku[index] != 0)                     //Проверяем на совпадение только ненулевые ячейки
            {
                //проверяем ячейку на совпадение значения с другими ячейками в строке, столбце, сегменте
                if (CheckPossibleValue(sudoku[index], index, sudoku) == false)
                    return false;
            }
        }
        return true;
    }

    //Метод поиска решения судоку
    static bool Solver(int index, ref int[] sudoku)
    {
        if ((index < 81) && (sudoku[index] == 0))   //Проверяем ячейки подряд, кроме заполненных и запрещаем выход за пределы массива судоку
        {
            Stack<int> stackPossibleValues = new Stack<int>();                  //инициализируем стек для возможных значений ячейки
            for (int possibleValue = 1; possibleValue < 10; possibleValue++)    //последовательно проверяем значение от 1 до 9 и записываем в стек
            {
                if (CheckPossibleValue(possibleValue, index, sudoku))           //если нет совпадений в столбце строке и сегменте
                    stackPossibleValues.Push(possibleValue);                    //записываем значение в стек
            }

            while (stackPossibleValues.Count > 0)           //проверяем значения из стека для ячейки, пока стек не опустеет
            {
                sudoku[index] = stackPossibleValues.Pop();  //присваеваем ячейке значение из стека
                if (Solver(index + 1, ref sudoku))          //Метод вызывает сам себя с увеличением индекса массива судоку
                    return true;                            //Условие if вернет true, только если достигли конца судоку, т.е. все ячейки заполнены
            }
            sudoku[index] = 0;      //если все значения стека проверены, и из вложенных методов возвращалось false, 
            return false;           //то обнуляем ячейку с номером index, возвращаем предыдущему методу false

        }
        else if (index < 81)        //заполненные ячейки пропускаем. 
        {
            if (Solver(index + 1, ref sudoku) == false)
                return false;
        }
        return true;                //возврат true будет при index = 81, т.е. все ячейки заполнены, 
                                    //произойдет последовательный выход из всех вложенных методов, т.к. все методы будут возвращать true
    }

    //Метод проверяее значение на совпадение с другими ячейками в строке, столбце, сегменте
    //в метод поступает: проверяемое значение, индекс ячейки, где проверяется это значение, и массив судоку
    static bool CheckPossibleValue(int value, int index, int[] sudoku)
    {
        //проверяем по строке и столбцу
        for (int j = 0; j < 9; j++)
        {
            if (value == sudoku[(index / 9) * 9 + j]    //находим первый элемент строки (index / 9) * 9
                & index != (index / 9) * 9 + j)         //условие чтобы ячейка не сравнивала себя с собой
            {
                return false;                           //возвращаем false, если было совпадение значения с ячейками в строке
            }

            if (value == sudoku[(index % 9) + 9 * j]    //находим первый элемент столбца (index % 9) + 9
                & index != (index % 9) + 9 * j)
            {
                return false;                           //возвращаем false, если было совпадение значения с ячейками в столбце
            }
        }
        //проверяем по сегменту
        for (int j = 0; j < 3; j++)
        {
            for (int k = 0; k < 3; k++)
            {                                           //находим первый элемент сегмента ((index / 27 * 27) + (index % 9) / 3 * 3)
                if (value == sudoku[(index / 27 * 27) + ((index % 9) / 3 * 3) + (9 * j) + k]
                    & index != ((index / 27 * 27) + (index % 9) / 3 * 3 + (9 * j) + k))
                {
                    return false;                       //возвращаем false, если было совпадение значения с ячейками в сегменте
                }
            }
        }
        return true;                                    //возвращаем true, если совпадений не было
    }

    //Просто метод вывода на экран судоку в удобоваримом виде
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

