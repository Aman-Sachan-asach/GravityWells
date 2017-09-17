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
	public GameObject spaceStationPrefab1;
	public GameObject moonPrefab1;
	public GameObject planetPrefab1;
	public GameObject playerShipPrefab;

	// Use this for initialization
	void Start () {}

	public void generateAllObjectForScene(int scenario)
	{
		generateBlackholes (scenario);
		generateSpaceStations (scenario);
		generatePlayerShip (scenario);
        generatePlanets (scenario);
        generateMoons (scenario);
    }

	public void generateBlackholes(int scenario)
	{
		if (scenario == 1 || scenario == 2)
		{
			Vector3 pos = new Vector3(-30.0f,0.0f,0.0f);
			GameObject tempblackhole = Instantiate(blackholePrefab1, pos, Quaternion.identity) as GameObject;
			blackholes.Add(tempblackhole);
		}
	}

	public void generateSpaceStations(int scenario)
	{
		if (scenario == 2)
		{
			Vector3 pos = new Vector3(-45.0f,0.0f,0.0f);
			GameObject tempspaceStation = Instantiate (spaceStationPrefab1, pos, Quaternion.identity * Quaternion.Euler (0, 0, -90.0f));
			tempspaceStation.GetComponent<SpaceStationScript> ().orbitcenter = new Vector3(-30.0f,0.0f,0.0f);
			spaceStations.Add(tempspaceStation);
		}
	}

	public void generatePlayerShip(int scenario)
	{
		if (scenario == 1 || scenario == 2) 
		{
			Vector3 pos = new Vector3 (30.0f, 1.8f, 25.0f);
			Quaternion rot = Quaternion.identity;
			rot *= Quaternion.Euler (0, -90, 0);  //face -x axis
			GameObject playerShip = Instantiate (playerShipPrefab, pos, rot) as GameObject;
		}
	}

    public void generatePlanets(int scenario)
    {
        if (scenario == 1 || scenario == 2)
        {
            Vector3 pos = new Vector3(-30.0f, 0.0f, -100.0f);
            GameObject tempPlanet = Instantiate(planetPrefab1, pos, Quaternion.identity) as GameObject;
            planets.Add(tempPlanet);
        }
    }

    public void generateMoons(int scenario)
    {
        if (scenario == 1 || scenario == 2)
        {
            Vector3 pos = new Vector3(-40.0f, 0.0f, -100.0f);
            GameObject tempMoon = Instantiate(moonPrefab1, pos, Quaternion.identity) as GameObject;
            tempMoon.GetComponent<MoonScript>().orbitcenter = new Vector3(-30.0f, 0.0f, -100.0f);
            moons.Add(tempMoon);
        }
    }

    public void positionPlayerShip(Vector3 pos)
	{
		playerShip.transform.position = pos;
	}

	// Update is called once per frame
	void Update () {}
}
