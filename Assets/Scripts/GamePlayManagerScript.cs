using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePlayManagerScript : MonoBehaviour 
{
	public GameObject objectGenerator;
	ObjectGeneratorScript generatorScript;
	public int scenario; //used to change the scene setup

	// Use this for initialization
	void Start () 
	{
		generatorScript = objectGenerator.GetComponent<ObjectGeneratorScript>();
		InitializeObjectPositions();
	}

	void InitializeObjectPositions()
	{
		generatorScript.generateAllObjectForScene(scenario);
	}

	// Update is called once per frame
	void Update () 
	{}
}
