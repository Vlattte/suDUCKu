using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NoteComponentActivator : MonoBehaviour
{
    public TextMeshProUGUI TextComponent;
    public Image ImageComponent;

    public void ActivateComponent(string name)
    {
        switch (name)
        {
            case "text":
                TextComponent.gameObject.SetActive(true);
                break;
            case "image":
                ImageComponent.gameObject.SetActive(true);
                break;
        }
    }
}
