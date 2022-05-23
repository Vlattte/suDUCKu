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

	public void PlayPressed()
	{
		SceneManager.LoadScene("GameScene", LoadSceneMode.Single);
	}

	public void LoadMainScene()
    {
		SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
    }

	public void RestartGame()
    {
		GameObject.FindGameObjectWithTag("SudokuGenerator").GetComponent<SudokuGenerator>().GenerateAgain();
    }

	public void ExitPressed()
	{
		Application.Quit();
	}
}
