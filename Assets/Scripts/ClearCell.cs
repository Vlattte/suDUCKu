using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearCell : MonoBehaviour
{
    private SudokuGrid GridManager;
    private void Start()
    {
        GridManager = GameObject.FindGameObjectWithTag("GridManager").GetComponent<SudokuGrid>();
    }
   
    private void OnMouseDown()
    {
        GridManager.ClearCell();
    }
}
