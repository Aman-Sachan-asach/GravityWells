using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class TitleScreenScript : MonoBehaviour
{
	public void NewGame()
	{
        SceneManager.LoadScene("Level0");
    }

	public void ContinueGame()
	{
        //read player pref to determine what the last level you were on is called
        int currentLevel = PlayerPrefs.GetInt("Level");

        string level = "Level";
        level += currentLevel.ToString();
        SceneManager.LoadScene(level);
	}

	public void HighScore()
	{
		Debug.Log ("You should implement a high score screen.");
	}

	public void ExitGame()
	{
		Application.Quit();
		Debug.Log ("Application.Quit() only works in build, not in editor");
	}
}
