using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartConroller : MonoBehaviour
{
    private int lives;                  //possible mistakes before game over
    public List<GameObject> hearts;
    public Sprite noHeartSprite;
    public Sprite HeartSprite;

    public GameObject LosePanelObj;     // Panel, that will apear after lose

    // Start is called before the first frame update
    void Start()
    {
        lives = 3;
    }

    public void decreaseLives()
    {
        lives--;
        if (lives <= 0)
        {
            hearts[lives].GetComponent<SpriteRenderer>().sprite = noHeartSprite;
            LosePanelObj.SetActive(false);
            return;
        }
        hearts[lives].GetComponent<SpriteRenderer>().sprite = noHeartSprite;
    }

    public void increaseLives(int count)
    {
        for(int i = 0; i < count; i++)
        {
            hearts[i].GetComponent<SpriteRenderer>().sprite = HeartSprite;
            LosePanelObj.SetActive(false);
        }
    }

    public int GetLives()
    {
        return lives;
    }
}
