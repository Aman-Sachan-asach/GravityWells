using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;

public class GamePlayManagerScript : MonoBehaviour 
{
    public GameObject objectGenerator;
    public GameObject gameOver;
    public GameObject stageCleared;
    public Slider fuelBar;
    public Text Timer;
    TimerScript timerScript;
    ObjectGeneratorScript generatorScript;
	public int scenario; //used to change the scene setup
    public int maxScenarios;
    
	// Use this for initialization
	void Start () 
	{
        gameOver.SetActive(false);

        timerScript = Timer.GetComponent<TimerScript>();
        generatorScript = objectGenerator.GetComponent<ObjectGeneratorScript>();
		InitializeObjectPositions();
	}

    public void HighScoreCheckandSet()
    {
        string levelmins = "Level" + scenario.ToString() + "minutes";
        string levelsecs = "Level" + scenario.ToString() + "seconds";

        //print("in score check");

        if (PlayerPrefs.HasKey(levelmins))
        {
            //HighScore Exists so check score against it
            if( timerScript.minutes < PlayerPrefs.GetFloat("levelmins") )
            {
                if( timerScript.seconds < PlayerPrefs.GetFloat("levelsecs") )
                {
                    print("update level scores");
                    PlayerPrefs.SetFloat(levelmins, timerScript.minutes);
                    PlayerPrefs.SetFloat(levelsecs, timerScript.seconds);
                }
            }
        }
        else
        {
            //print("create new level scores");
            PlayerPrefs.SetFloat(levelmins, timerScript.minutes);
            PlayerPrefs.SetFloat(levelsecs, timerScript.seconds);
        }
    }

	void InitializeObjectPositions()
	{
		generatorScript.generateAllObjectForScene(scenario);
	}

    public void ResetCurrentLevel()
    {
        HighScoreCheckandSet();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void NextLevel()
    {
        if ( (scenario+1) < maxScenarios )
        {
            scenario++;
            if (scenario > PlayerPrefs.GetInt("Level")) ;
            {
                PlayerPrefs.SetInt("Level", scenario);
            }

            string level = "Level";
            level += scenario.ToString();
            HighScoreCheckandSet();
            SceneManager.LoadScene(level);
        }
        else
        {
            print("No more Levels :(");
            PlayerPrefs.SetInt("Level", 0);
            SceneManager.LoadScene("Start Scene");
        }
    }

    //Coroutines are stalled indefinetly be called once the object that contains it has been destroyed or made inactive
    public IEnumerator GameOverandReset()
    {
        //game over condition
        gameOver.SetActive(true);

        //check for highscore

        print("here in coroutine");
        yield return new WaitForSeconds(3);
        print("after 3 seconds");
        //Reset Level
        ResetCurrentLevel();
    }
}
