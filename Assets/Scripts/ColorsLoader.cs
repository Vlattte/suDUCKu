using UnityEngine;
using UnityEngine.UI;

public class ColorsLoader : MonoBehaviour
{
    string saveFilePath;
    ColorStruct colors;
    public Image sudokuImage;

    private void Start()
    {
        saveFilePath = Application.persistentDataPath + "/Colors.dat";
        ColorChanger();
        if (sudokuImage != null)
            sudokuImage.color
                = new Color(colors.sudokuBackColor[0], colors.sudokuBackColor[1], colors.sudokuBackColor[2]);
    }

    private void Update()
    {
        //to background color
        if(DataHolder.isTimeToLoadColors[0] == true)
        {
            ColorChanger();
            //DataHolder.isTimeToLoadColors[0] = false;
        }

        //to sudoku background color
        if(DataHolder.isTimeToLoadColors[1] == true)
        {
            if (sudokuImage!=null)
                sudokuImage.color 
                    = new Color(colors.sudokuBackColor[0], colors.sudokuBackColor[1], colors.sudokuBackColor[2]);
            //DataHolder.isTimeToLoadColors[1] = false;
        }
    }

    void ColorChanger()
    {
        colors = SaveSystem.LoadData<ColorStruct>(saveFilePath);
        if(colors.backGroundColor!=null)
            this.GetComponent<Image>().color = new Color(colors.backGroundColor[0], colors.backGroundColor[1],
                colors.backGroundColor[2]);
    }
}
