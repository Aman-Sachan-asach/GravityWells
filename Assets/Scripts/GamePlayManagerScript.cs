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
    ObjectGeneratorScript generatorScript;
	public int scenario; //used to change the scene setup

	// Use this for initialization
	void Start () 
	{
        //scenario = PlayerPrefs.GetInt("Level");

        gameOver.SetActive(false);

        generatorScript = objectGenerator.GetComponent<ObjectGeneratorScript>();
		InitializeObjectPositions();
	}

	void InitializeObjectPositions()
	{
		generatorScript.generateAllObjectForScene(scenario);
	}

    public void ResetCurrentLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void NextLevel()
    {
        scenario++;

        int currentLevel = scenario;
        if ( currentLevel > PlayerPrefs.GetInt("Level") );
        {
            PlayerPrefs.SetInt("Level", currentLevel);
        }

        string level = "Level";
        level += scenario.ToString();
        SceneManager.LoadScene(level);
    }

    //Coroutines are stalled indefinetly be called once the object that contains it has been destroyed or made inactive
    public IEnumerator GameOverandReset()
    {
        //game over condition
        gameOver.SetActive(true);

        print("here in coroutine");
        yield return new WaitForSeconds(3);
        print("after 3 seconds");
        //Reset Level
        ResetCurrentLevel();
    }
}
