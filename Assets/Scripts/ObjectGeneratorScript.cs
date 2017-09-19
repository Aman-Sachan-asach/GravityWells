using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectGeneratorScript : MonoBehaviour 
{
	public List<GameObject> blackholes = new List<GameObject>();
    public List<GameObject> repulsiveStars = new List<GameObject>();
    public List<GameObject> planets = new List<GameObject>();
	public List<GameObject> moons = new List<GameObject>();
	public List<GameObject> spaceStations = new List<GameObject>();
	public GameObject playerShip;

	public GameObject blackholePrefab1;
    public GameObject repulsiveStarPrefab1;
    public GameObject spaceStationPrefab1;
	public GameObject moonPrefab1;
	public GameObject planetPrefab1;
	public GameObject playerShipPrefab;

    Vector3 min = new Vector3(0, 0, 0);
    Vector3 max = new Vector3(0, 0, 0);
    public Bounds sceneBounds;

	public void generateAllObjectForScene(int scenario)
	{
        generatePlayerShip(scenario);

        generateBlackholes (scenario);        
        generateSpaceStations (scenario);
        generateRepulsiveStars(scenario);

        generatePlanets (scenario);
        generateMoons (scenario);

        //print("ogs " + sceneBounds.min + " " + sceneBounds.max);
    }

	public void generateBlackholes(int scenario)
	{
		if (scenario == 0 || scenario == 1)
		{
			Vector3 pos = new Vector3(-30.0f,0.0f,0.0f);
			GameObject tempblackhole = Instantiate(blackholePrefab1, pos, Quaternion.identity) as GameObject;
            compareWithBounds(pos);
            blackholes.Add(tempblackhole);
		}
	}

	public void generateSpaceStations(int scenario)
	{
		if (scenario == 0)
		{
			Vector3 pos = new Vector3(-45.0f,0.0f,0.0f);
			GameObject tempspaceStation = Instantiate (spaceStationPrefab1, pos, Quaternion.identity * Quaternion.Euler (0, 0, -90.0f));
			tempspaceStation.GetComponent<SpaceStationScript> ().orbitcenter = new Vector3(-30.0f,0.0f,0.0f);
            compareWithBounds(pos);
            spaceStations.Add(tempspaceStation);
		}
	}

    public void generateRepulsiveStars(int scenario)
    {
        if (scenario == 0 || scenario == 1)
        {
            Vector3 pos = new Vector3(-30.0f, 0.0f, -60.0f);
            GameObject tempstar = Instantiate(repulsiveStarPrefab1, pos, Quaternion.identity) as GameObject;
            compareWithBounds(pos);
            repulsiveStars.Add(tempstar);
        }
    }

    public void generatePlayerShip(int scenario)
	{
		if (scenario == 0 || scenario == 1) 
		{
			Vector3 pos = new Vector3 (30.0f, 1.8f, 25.0f);
			Quaternion rot = Quaternion.identity;
			rot *= Quaternion.Euler (0, -90, 0);  //face -x axis
            compareWithBounds(pos);
            playerShip = Instantiate (playerShipPrefab, pos, rot) as GameObject;
		}
	}

    public void generatePlanets(int scenario)
    {
        if (scenario == 0 || scenario == 1)
        {
            Vector3 pos = new Vector3(-30.0f, 0.0f, -100.0f);
            GameObject tempPlanet = Instantiate(planetPrefab1, pos, Quaternion.identity) as GameObject;
            compareWithBounds(pos);
            planets.Add(tempPlanet);
        }
    }

    public void generateMoons(int scenario)
    {
        if (scenario == 0 || scenario == 1)
        {
            Vector3 pos = new Vector3(-40.0f, 0.0f, -100.0f);
            GameObject tempMoon = Instantiate(moonPrefab1, pos, Quaternion.identity) as GameObject;
            tempMoon.GetComponent<MoonScript>().orbitcenter = new Vector3(-30.0f, 0.0f, -100.0f);
            compareWithBounds(pos);
            moons.Add(tempMoon);
        }
    }

    public void compareWithBounds(Vector3 pos)
    {
        min.x = Mathf.Min(pos.x, sceneBounds.min.x);
        min.y = Mathf.Min(pos.y, sceneBounds.min.y);
        min.z = Mathf.Min(pos.z, sceneBounds.min.z);

        max.x = Mathf.Max(pos.x, sceneBounds.max.x);
        max.y = Mathf.Max(pos.y, sceneBounds.max.y);
        max.z = Mathf.Max(pos.z, sceneBounds.max.z);

        sceneBounds.SetMinMax(min, max);
    }
}
