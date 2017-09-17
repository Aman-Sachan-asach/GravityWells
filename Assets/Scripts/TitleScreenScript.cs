using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleScreenScript : MonoBehaviour
{
	// Use this for initialization
	void Start () {}
	
	// Update is called once per frame
	void Update () {}

	public void NewGame()
	{
		Application.LoadLevel("Lvl001");
	}

	public void ContinueGame()
	{
		//read file to determine what the last level you were on is called
		Application.LoadLevel("Lvl001");
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
