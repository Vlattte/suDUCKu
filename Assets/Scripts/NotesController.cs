using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NotesController : MonoBehaviour
{
    public TextMeshProUGUI[] notes;

    public void SetNoteActive(bool isActive, int number)
    {
        //сделай чтобы компонент становиться не активным, а не балуйся с цветами
        Color cur_color_comp = notes[number - 1].color;
        /*if (isActive)
            notes[number - 1].GetComponent<Image>().color = new Color(cur_color_comp.r, cur_color_comp.g, cur_color_comp.b, 1f);
        else
            notes[number - 1].GetComponent<Image>().color = new Color(cur_color_comp.r, cur_color_comp.g, cur_color_comp.b, 0f);*/
        if (isActive)
            notes[number - 1].color = new Color(cur_color_comp.r, cur_color_comp.g, cur_color_comp.b, 1f);
        else
            notes[number - 1].color = new Color(cur_color_comp.r, cur_color_comp.g, cur_color_comp.b, 0f);
    }
}
