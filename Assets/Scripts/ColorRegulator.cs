using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using TMPro;


public class ColorRegulator : MonoBehaviour
{
    //sliders
    public Slider redSlider;
    public Slider greenSlider;
    public Slider blueSlider;

    //text objects
    public TextMeshProUGUI redText;
    public TextMeshProUGUI greenText;
    public TextMeshProUGUI blueText;

    public Image resultColorImg;
    [SerializeField] private ColorStruct colors;
    [SerializeField] private string saveFilePath;
    public string whatColorStr;

    //defalut colors
    private Color defaultSudokuBackground;
    private Color defaultBackground;

    void Start()
    {
        //set default colors
        defaultBackground = new Color(0.4784314f, 0.6235294f, 0.8392157f);
        defaultSudokuBackground = new Color(0.7411765f, 0.5490196f, 0.3960784f);

        saveFilePath = Application.persistentDataPath + "/Colors.dat";

        whatColorStr = " ";

        if (File.Exists(saveFilePath))
        {
            colors = SaveSystem.LoadData<ColorStruct>(saveFilePath);
        }
         

        redSlider.onValueChanged.AddListener((v) =>
        {
            redText.text = v.ToString("0");
            resultColorImg.color = new Color(redSlider.value/255, greenSlider.value / 255, blueSlider.value / 255);
        });

        greenSlider.onValueChanged.AddListener((v) =>
        {
            greenText.text = v.ToString("0");
            resultColorImg.color = new Color(redSlider.value / 255, greenSlider.value / 255, blueSlider.value / 255);
        });

        blueSlider.onValueChanged.AddListener((v) =>
        {
            blueText.text = v.ToString("0");
            resultColorImg.color = new Color(redSlider.value / 255, greenSlider.value / 255, blueSlider.value / 255);
        });
    }

    public void SetSliders(Color _col)
    {
        resultColorImg.color = new Color(_col.r, _col.g, _col.b);

        //set text values
        redText.text = (_col.r * 255).ToString("0");
        greenText.text = (_col.g * 255).ToString("0");
        blueText.text = (_col.b * 255).ToString("0");

        //set sliders
        redSlider.value = _col.r * 255;
        greenSlider.value = _col.g * 255;
        blueSlider.value = _col.b * 255;
    }

    public void SaveColor()
    {
        if (DataHolder.whatColorsChangedStr == "BackColor")
        {
            colors.backGroundColor = new float[3] { resultColorImg.color.r, resultColorImg.color.g, resultColorImg.color.b };
            DataHolder.isTimeToLoadColors[0] = true;
        }

        if (DataHolder.whatColorsChangedStr == "SudokuColor")
        {
            colors.sudokuBackColor = new float[3] { resultColorImg.color.r, resultColorImg.color.g, resultColorImg.color.b };
            DataHolder.isTimeToLoadColors[1] = true;
        }
            
        SaveSystem.SaveData(saveFilePath, colors);
    }

    public void SetDefaultColor()
    {
        if (DataHolder.whatColorsChangedStr == "BackColor")
        {
            resultColorImg.color = defaultBackground;
            //colors.sudokuBackColor = new float[3] { defaultBackground.r, defaultBackground.g, defaultBackground.b };
        }

        if (DataHolder.whatColorsChangedStr == "SudokuColor")
        {
            resultColorImg.color = defaultSudokuBackground;
            //colors.sudokuBackColor = new float[3] { defaultSudokuBackground.r, defaultSudokuBackground.g, defaultSudokuBackground.b };
        }

        //SaveSystem.SaveData(saveFilePath, colors);
    }
}
