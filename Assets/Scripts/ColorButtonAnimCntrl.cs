using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;


public class ColorButtonAnimCntrl : MonoBehaviour
{
    public GameObject BackColor;
    public GameObject SudokuColor;
    private bool isHidden;
    public GameObject colorRegulators;
    public Image resultColorImage;
    public ColorRegulator regulator;
    public GameObject returnButton;

    private void Start()
    {
        //if buttons aren't hidden, it's false
        //if they are hidden and animation is "up", it's true
        isHidden = false;
    }

    public void HideShowOtherButtons(string thisBtn)
    {
        
        GameObject curButton = BackColor;
        if (thisBtn != "BackColor")
        {
            curButton = BackColor;
        }
        if (thisBtn != "SudokuColor")
        {
            curButton = SudokuColor;
        }

        curButton.SetActive(isHidden);

        colorRegulators.SetActive(!isHidden);
        returnButton.SetActive(isHidden);

        if (!isHidden)
        {
            string saveFilePath = Application.persistentDataPath + "/Colors.dat";


            if (thisBtn == "BackColor")
            {
                //load color of background, that was chosen before
                if (File.Exists(saveFilePath))
                {
                    ColorStruct colors = SaveSystem.LoadData<ColorStruct>(saveFilePath);
                    if (colors.backGroundColor != null)
                        regulator.SetSliders(new Color(colors.backGroundColor[0], colors.backGroundColor[1], colors.backGroundColor[2]));
                    else
                        regulator.SetSliders(Color.black);
                }
            }

            if (thisBtn == "SudokuColor")
            {
                //load color of sudoku, that was chosen before
                if (File.Exists(saveFilePath))
                {
                    ColorStruct colors = SaveSystem.LoadData<ColorStruct>(saveFilePath);
                    if (colors.sudokuBackColor != null)
                        regulator.SetSliders(new Color(colors.sudokuBackColor[0], colors.sudokuBackColor[1], colors.sudokuBackColor[2]));
                    else
                        regulator.SetSliders(Color.black);
                }
            }
        }

        //tells to saver what color will be changed
        if (isHidden == false)
        {
            DataHolder.whatColorsChangedStr = thisBtn;
        }

        isHidden = !isHidden;
    }
}
