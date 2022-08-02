using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;

public class ContinueButtonScript : MonoBehaviour
{
    string saveFilePath;

    void Start()
    {
        saveFilePath = Application.persistentDataPath + "/Save.dat";
        if (!File.Exists(saveFilePath))
        {
            this.GetComponent<Image>().color = new Color(0.6784314f, 0.6509804f, 0.6509804f);
        }
    }

    private void Update()
    {
        if (!File.Exists(saveFilePath))
        {
            this.GetComponent<Image>().color = new Color(0.6784314f, 0.6509804f, 0.6509804f);
            this.GetComponent<Button>().interactable = false;
        }
        else
        {
            this.GetComponent<Image>().color = Color.white;
            this.GetComponent<Button>().interactable = true;
        }
    }
}
