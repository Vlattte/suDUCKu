using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DataHolder
{
    private static int difficulty = 47;

    public static int ManageDifficulty
    {
        get { return difficulty; }
        set { difficulty = value; }
    }
}
