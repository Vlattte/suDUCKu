using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{
	
	public void SetDifficulty(int _difficulty)
    {
		DataHolder.ManageDifficulty = _difficulty;
    }

	// нопки дл€ главного меню
	public void PlayPressed()
	{
		SceneManager.LoadScene("GameScene", LoadSceneMode.Single);
	}

	public void ExitPressed()
	{
		Application.Quit();
	}
}
