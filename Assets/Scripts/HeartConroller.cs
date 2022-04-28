using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartConroller : MonoBehaviour
{
    private int lives;      //possible mistakes before game over
    public List<GameObject> hearts;
    public Sprite noHeartPrefab;

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
            hearts[lives].GetComponent<SpriteRenderer>().sprite = noHeartPrefab;
            Application.Quit();
            return;
        }
        hearts[lives].GetComponent<SpriteRenderer>().sprite = noHeartPrefab;

    }
}
