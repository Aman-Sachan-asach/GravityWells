using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectGeneratorScript : MonoBehaviour 
{
	public List<GameObject> blackholes = new List<GameObject>();
	public List<GameObject> planets = new List<GameObject>();
	public List<GameObject> moons = new List<GameObject>();
	public List<GameObject> spaceStations = new List<GameObject>();
	public GameObject playerShip;

	public GameObject blackholePrefab1;
	public GameObject MoonPrefab1;
	public GameObject planetPrefab1;
	public GameObject playerShipPrefab;
	// Use this for initialization
	void Start ()
	{}

	public void generateBlackholes()
	{
		Vector3 pos = new Vector3(-30.0f,0.0f,0.0f);
		GameObject tempblackhole = Instantiate(blackholePrefab1, pos, Quaternion.identity) as GameObject;
		blackholes.Add(tempblackhole);
	}

	public void generatePlayerShip(Vector3 pos)
	{
		Quaternion rot = Quaternion.identity;
		rot *= Quaternion.Euler (0, -90, 0);  //face -x axis
		GameObject playerShip = Instantiate(playerShipPrefab, pos, rot) as GameObject;
	}

	public void positionPlayerShip(Vector3 pos)
	{
		playerShip.transform.position = pos;
	}

	// Update is called once per frame
	void Update () {
		
	}
}
