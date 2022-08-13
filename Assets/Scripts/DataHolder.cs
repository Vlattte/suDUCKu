using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct ModesStruct
{
    public bool isHints;
    public bool isTimer;
    public bool isLives;
    public bool isMistakes;
}

public static class DataHolder
{
    private static int difficulty = 47;
    private static bool isContinuePressed = false;

    //modes parameters
    private static ModesStruct modes;
    private static bool isModesInit = false;
    private static ModesStruct tempModes;
    public static bool isModeChanged;

    //colors parametres
    public static string whatColorsChangedStr;  //what colors was changed
    public static bool isColorsInit = false;

    public static bool[] isTimeToLoadColors = new bool[2];

    public static void ResetModes()
    {
        isModesInit = true;

        modes.isHints = false;
        modes.isTimer = false;
        modes.isLives = true;
        modes.isMistakes = true;
    }

    //returns modes of current game, if player decided to not start new game,
    //but continue current
    public static void ReturnSavedModes()
    {
        modes = tempModes;
    }

    public static void  SaveModesInBuf()
    {
        tempModes = modes;
        ResetModes();
        isModeChanged = true;
    }

    public static void SetSudokuModesToOposite(string mode)
    {
        if(!isModesInit)
        {
            isModesInit = true;

            modes.isHints = false;
            modes.isTimer = false;
            modes.isLives = true;
            modes.isMistakes = true;
            isModeChanged = false;
        }


        switch (mode)
        {
            case "HINTS":
                modes.isHints = !modes.isHints;
                break;
            case "TIMER":
                modes.isTimer = !modes.isTimer;
                break;
            case "LIVES":
                modes.isLives = !modes.isLives;
                break;
            case "MISTAKES":
                modes.isMistakes = !modes.isMistakes;
                isModeChanged = true;
                break;
        }
    }

    public static ModesStruct ManageModes
    {
        get { return modes; }
        set { modes = value; }
    }

    public static int ManageDifficulty
    {
        get { return difficulty; }
        set { difficulty = value; }
    }

    public static bool IsContinueMode
    {
        get { return isContinuePressed; }
        set { isContinuePressed = value; }
    }
}
