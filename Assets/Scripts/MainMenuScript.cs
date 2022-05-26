using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public class MainMenuScript : MonoBehaviour
{
    public void SetDifficulty(int _difficulty)
    {
		DataHolder.ManageDifficulty = _difficulty;
    }

	public void SetDiffilcultyAndEraiseNumbers(int _difficulty)
    {
		GameObject.FindGameObjectWithTag("SudokuGenerator").GetComponent<SudokuGenerator>().EraisingWithNewDifficulty(_difficulty);
	}

	public void LoadGameScene()
	{
		SceneManager.LoadScene("GameScene", LoadSceneMode.Single);
	}

	public void LoadMainScene()
    {
		GameObject.FindGameObjectWithTag("GridManager").GetComponent<SuduckuGrid>().SaveCurCells();
		SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
    }

	public void ContinueGame()
    {
		string saveFilePath = Application.persistentDataPath + "/Save.dat";
		if(File.Exists(saveFilePath))
        {
			DataHolder.ManagePlayMode = true;
			SceneManager.LoadScene("GameScene", LoadSceneMode.Single);
		}
	}

	public void NewGame()
    {
		GameObject.FindGameObjectWithTag("SudokuGenerator").GetComponent<SudokuGenerator>().NewGameGenerator();
	}

	public void RestartGame()
    {
		GameObject.FindGameObjectWithTag("GridManager").GetComponent<SuduckuGrid>().RestartGame();
	}

	public void ExitPressed()
	{
		Application.Quit();
	}
}
