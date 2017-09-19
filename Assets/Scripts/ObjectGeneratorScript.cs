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
    public GameObject blackholePrefab2;
    public GameObject repulsiveStarPrefab1;
    public GameObject repulsiveStarPrefab2;
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
    }

	public void generateBlackholes(int scenario)
	{
		if (scenario == 0 || scenario == 2)
		{
			Vector3 pos = new Vector3(-30.0f,0.0f,0.0f);
			GameObject tempblackhole = Instantiate(blackholePrefab1, pos, Quaternion.identity) as GameObject;
            compareWithBounds(pos);
            blackholes.Add(tempblackhole);
		}
        else if (scenario == 3)
        {
            Vector3 pos = new Vector3(-43.6f, 0.0f, 85.7f);
            GameObject tempblackhole = Instantiate(blackholePrefab1, pos, Quaternion.identity) as GameObject;
            compareWithBounds(pos);
            blackholes.Add(tempblackhole);
        }
        else if (scenario == 4)
        {
            Vector3 pos = new Vector3(-61.4f, 0.0f, -3.6f);
            GameObject tempblackhole = Instantiate(blackholePrefab1, pos, Quaternion.identity) as GameObject;
            compareWithBounds(pos);
            blackholes.Add(tempblackhole);

            pos = new Vector3(-69.2f, 0.0f, 99.5f);
            tempblackhole = Instantiate(blackholePrefab2, pos, Quaternion.identity) as GameObject;
            compareWithBounds(pos);
            blackholes.Add(tempblackhole);
        }
    }

	public void generateSpaceStations(int scenario)
	{
		if (scenario == 2)
        {
            Vector3 pos = new Vector3(-45.0f, 0.0f, 0.0f);
            GameObject tempspaceStation = Instantiate(spaceStationPrefab1, pos, Quaternion.identity * Quaternion.Euler(0, 0, -90.0f));
            tempspaceStation.GetComponent<SpaceStationScript>().orbitcenter = new Vector3(-30.0f, 0.0f, 0.0f);
            compareWithBounds(pos);
            spaceStations.Add(tempspaceStation);
		}
        else if (scenario == 3)
        {
            Vector3 pos = new Vector3(-58.6f, 0.0f, 85.7f);
            GameObject tempspaceStation = Instantiate(spaceStationPrefab1, pos, Quaternion.identity * Quaternion.Euler(0, 0, -90.0f));
            tempspaceStation.GetComponent<SpaceStationScript>().orbitcenter = new Vector3(-43.6f, 0.0f, 85.7f);
            compareWithBounds(pos);
            spaceStations.Add(tempspaceStation);
        }
        else if (scenario == 4)
        {
            Vector3 pos = new Vector3(-84.2f, 0.0f, 99.5f);
            GameObject tempspaceStation = Instantiate(spaceStationPrefab1, pos, Quaternion.identity * Quaternion.Euler(0, 0, -90.0f));
            tempspaceStation.GetComponent<SpaceStationScript>().orbitcenter = new Vector3(-69.2f, 0.0f, 99.5f);
            compareWithBounds(pos);
            spaceStations.Add(tempspaceStation);
        }
    }

    public void generateRepulsiveStars(int scenario)
    {
        if (scenario == 1)
        {
            Vector3 pos = new Vector3(10.4f, 0.0f, 62.3f);
            GameObject tempstar = Instantiate(repulsiveStarPrefab1, pos, Quaternion.identity) as GameObject;
            compareWithBounds(pos);
            repulsiveStars.Add(tempstar);
        }
        else if (scenario == 3)
        {
            Vector3 pos = new Vector3(-16.9f, 0.0f, -45.3f);
            GameObject tempstar = Instantiate(repulsiveStarPrefab1, pos, Quaternion.identity) as GameObject;
            compareWithBounds(pos);
            repulsiveStars.Add(tempstar);
        }
        else if (scenario == 4)
        {
            Vector3 pos = new Vector3(-50.1f, 0.0f, -66.9f);
            GameObject tempstar = Instantiate(repulsiveStarPrefab1, pos, Quaternion.identity) as GameObject;
            compareWithBounds(pos);
            repulsiveStars.Add(tempstar);

            pos = new Vector3(-151.6f, 0.0f, -49.1f);
            tempstar = Instantiate(repulsiveStarPrefab2, pos, Quaternion.identity) as GameObject;
            compareWithBounds(pos);
            repulsiveStars.Add(tempstar);
        }
    }

    public void generatePlayerShip(int scenario)
	{
		if (scenario <= 3) 
		{
			Vector3 pos = new Vector3 (30.0f, 1.8f, 25.0f);
			Quaternion rot = Quaternion.identity;
			rot *= Quaternion.Euler (0, -90, 0);  //face -x axis
            compareWithBounds(pos);
            playerShip = Instantiate (playerShipPrefab, pos, rot) as GameObject;
		}
        else if (scenario == 4)
        {
            Vector3 pos = new Vector3(-10.1f, 0.0f, 128.0f);
            Quaternion rot = Quaternion.identity;
            rot *= Quaternion.Euler(0, -90, 0);  //face -x axis
            compareWithBounds(pos);
            playerShip = Instantiate(playerShipPrefab, pos, rot) as GameObject;
        }
    }

    public void generatePlanets(int scenario)
    {
        if (scenario <= 2)
        {
            Vector3 pos = new Vector3(-45.0f, 0.0f, -100.0f);
            GameObject tempPlanet = Instantiate(planetPrefab1, pos, Quaternion.identity) as GameObject;
            compareWithBounds(pos);
            planets.Add(tempPlanet);
        }
        else if (scenario == 3 || scenario == 4)
        {
            Vector3 pos = new Vector3(-106.6f, 0.0f, -101.1f);
            GameObject tempPlanet = Instantiate(planetPrefab1, pos, Quaternion.identity) as GameObject;
            compareWithBounds(pos);
            planets.Add(tempPlanet);
        }
    }

    public void generateMoons(int scenario)
    {
        if (scenario <= 2)
        {
            Vector3 pos = new Vector3(-55.0f, 0.0f, -100.0f);
            GameObject tempMoon = Instantiate(moonPrefab1, pos, Quaternion.identity) as GameObject;
            tempMoon.GetComponent<MoonScript>().orbitcenter = new Vector3(-45.0f, 0.0f, -100.0f);
            compareWithBounds(pos);
            moons.Add(tempMoon);
        }
        else if(scenario == 3 || scenario == 4)
        {
            Vector3 pos = new Vector3(-121.6f, 0.0f, -116.1f);
            GameObject tempMoon = Instantiate(moonPrefab1, pos, Quaternion.identity) as GameObject;
            tempMoon.GetComponent<MoonScript>().orbitcenter = new Vector3(-106.6f, 0.0f, -101.1f);
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
