using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;

public class TitleScreenScript : MonoBehaviour
{
    public GameObject HighScore;
    public Text hsText;
    public GameObject HighScoreBackground; 
    public bool flag_inHighScoreMode = false;
    
    // Use this for initialization
    void Start()
    {
        hsText = HighScore.GetComponent<Text>() as Text;

        PlayerPrefs.SetInt("Level", 0);
    }

    public void NewGame()
	{
        PlayerPrefs.SetInt("Level", 0);
        SceneManager.LoadScene("Level0");
    }

	public void ContinueGame()
	{
        //read player pref to determine what the last level you were on is called
        int i = PlayerPrefs.GetInt("Level");
        print(i);
        string level = "Level";
        level += i.ToString();
        SceneManager.LoadScene(level);
	}

	public void DisplayHighScore()
	{
        HighScoreBackground.SetActive(true);
        HighScore.SetActive(true);
        flag_inHighScoreMode = true;

        string final = "";

        int i = 0;
        string levelmins = "Level" + i.ToString() + "minutes";
        string levelsecs = "Level" + i.ToString() + "seconds";
                
        while(PlayerPrefs.HasKey(levelmins))
        {
            float mins = PlayerPrefs.GetFloat(levelmins);
            float secs = PlayerPrefs.GetFloat(levelsecs);
            final += "Level " + i.ToString() + "      " + mins.ToString() + ":" + secs.ToString() + "\n";

            Debug.Log("minutes: " + mins);
            Debug.Log("seconds: " + secs);

            i++;
            levelmins = "Level" + i.ToString() + "minutes";
            levelsecs = "Level" + i.ToString() + "seconds";
        }
        
        hsText.text = "Best Times: \n" + final;
    }

    public void Update()
    {
        if(flag_inHighScoreMode)
        {
            if( Input.GetButton("Submit") )
            {
                HighScoreBackground.SetActive(false);
                HighScore.SetActive(false);
                flag_inHighScoreMode = false;
            }
        }
    }

	public void ExitGame()
	{
		Application.Quit();
		Debug.Log ("Application.Quit() only works in build, not in editor");
	}
}
