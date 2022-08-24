using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class PlayModeTests
{
    SudokuGrid gridManager;
    SudokuGenerator generator;
        
    [SetUp]
    public void SetUp()
    {
        gridManager = GameObject.Instantiate<SudokuGrid>(gridManager);
    }

    [UnityTest]
    public IEnumerator PlayModeTestsWithEnumeratorPasses()
    {
        // Use the Assert class to test conditions.
        // Use yield to skip a frame.
        yield return null;
    }
}
