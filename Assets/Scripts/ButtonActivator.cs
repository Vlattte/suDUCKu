using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonActivator : MonoBehaviour
{
    public bool isActivated;

    public void ChangeColorToOposite()
    {
        if(isActivated)
            this.gameObject.GetComponent<Image>().color = new Color(0.6784314f, 0.6509804f, 0.6509804f);
        else
            this.gameObject.GetComponent<Image>().color = new Color(1, 1, 1);

        isActivated = !isActivated;
    }
}
