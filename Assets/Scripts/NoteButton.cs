using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NoteButton : MonoBehaviour
{
    private SuduckuGrid GridManager;
    public GameObject pencilObj;
    private bool isLittleNums;


    private void Start()
    {
        GridManager = GameObject.FindGameObjectWithTag("GridManager").GetComponent<SuduckuGrid>();
        isLittleNums = false;
    }

    private void OnMouseDown()
    {
        GridManager.ChangeEnterMode();
        if (!isLittleNums)
        {
            isLittleNums = true;   
            pencilObj.transform.localScale /= 2;
        }
        else if (isLittleNums)
        {
            isLittleNums = false;
            pencilObj.transform.localScale *= 2;
        }
    }
}
