using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{
	// нопки дл€ главного меню
	public void PlayPressed()
	{
		SceneManager.LoadScene("GameScene");
	}

	public void ExitPressed()
	{
		Application.Quit();
	}
}
