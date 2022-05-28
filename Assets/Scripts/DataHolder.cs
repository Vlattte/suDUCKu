using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DataHolder
{
    private static int difficulty = 47;
    private static bool isContinuePressed = false;

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
