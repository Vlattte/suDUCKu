using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartConroller : MonoBehaviour
{
    private int lives;                  //possible mistakes before game over
    public List<GameObject> hearts;
    public Sprite noHeartSprite;
    public Sprite HeartSprite;

    // Start is called before the first frame update
    void Start()
    {
        lives = 3;
    }

    //return true  if no more hearts left
    //return false if there is some more hearts left
    public bool decreaseLives()
    {
        lives--;
        if (lives <= 0)
        {
            hearts[lives].GetComponent<SpriteRenderer>().sprite = noHeartSprite;
            return true;
        }
        hearts[lives].GetComponent<SpriteRenderer>().sprite = noHeartSprite;
        return false;
    }

    public void increaseLives(int count)
    {
        lives = count;
        for(int i = 0; i < count; i++)
        {
            hearts[i].GetComponent<SpriteRenderer>().sprite = HeartSprite;
        }
    }

    public int GetLives()
    {
        return lives;
    }
}
