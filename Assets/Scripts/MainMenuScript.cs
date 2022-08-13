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
		SetDifficulty(_difficulty);
		GameObject.FindGameObjectWithTag("SudokuGenerator").GetComponent<SudokuGenerator>().EraisingWithNewDifficulty(_difficulty);
	}

	public void LoadGameScene()
	{
		SceneManager.LoadScene("GameScene", LoadSceneMode.Single);
	}

	public void LoadMainScene()
    {
		DataHolder.IsContinueMode = false;
		GameObject.FindGameObjectWithTag("GridManager").GetComponent<SudokuGrid>().SaveCurCells();
		SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
    }

	public void ContinueGame()
    {
		string saveFilePath = Application.persistentDataPath + "/Save.dat";
		if(File.Exists(saveFilePath))
        {
			DataHolder.IsContinueMode = true;
			SceneManager.LoadScene("GameScene", LoadSceneMode.Single);
		}
	}

	public void NewGame()
    {
		DataHolder.IsContinueMode = false;
		GameObject.FindGameObjectWithTag("SudokuGenerator").GetComponent<SudokuGenerator>().NewGameGenerator();
	}

	public void RestartGame()
    {
		DataHolder.IsContinueMode = false;
		GameObject.FindGameObjectWithTag("GridManager").GetComponent<SudokuGrid>().RestartGame();
	}

	public void HintsSwicher()
    {
		DataHolder.SetSudokuModesToOposite("HINTS");
    }

	public void TimerSwicher()
	{
		DataHolder.SetSudokuModesToOposite("TIMER");
	}

	public void LivesSwicher()
	{
		DataHolder.SetSudokuModesToOposite("LIVES");
	}

	public void MistakesSwitcher()
    {
		DataHolder.SetSudokuModesToOposite("MISTAKES");
	}

	public void ResetModes()
    {
		DataHolder.ResetModes();
    }

	public void SaveModesInBuf()
    {
		DataHolder.SaveModesInBuf();
    }

	public void ReturnSavedModes()
    {
		DataHolder.ReturnSavedModes();
    }

	public void ExitPressed()
	{
		Application.Quit();
	}
}
