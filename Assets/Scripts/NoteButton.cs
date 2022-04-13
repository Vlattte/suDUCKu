using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteButton : MonoBehaviour
{
    private SuduckuGrid GridManager;
    private void Start()
    {
        GridManager = GameObject.FindGameObjectWithTag("GridManager").GetComponent<SuduckuGrid>();
    }

    private void OnMouseDown()
    {
        GridManager.ChangeEnterMode();
    }
}
