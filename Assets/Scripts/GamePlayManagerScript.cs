using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePlayManagerScript : MonoBehaviour 
{
	public GameObject objectGenerator;
	ObjectGeneratorScript generatorScript;

	// Use this for initialization
	void Start () 
	{
		generatorScript = objectGenerator.GetComponent<ObjectGeneratorScript>();
		InitializeObjectPositions();
	}

	void InitializeObjectPositions()
	{
		generatorScript.generateBlackholes();
		generatorScript.generatePlayerShip(new Vector3(30.0f, 0.25f, 25.0f));
	}

	// Update is called once per frame
	void Update () 
	{
		
	}
}
