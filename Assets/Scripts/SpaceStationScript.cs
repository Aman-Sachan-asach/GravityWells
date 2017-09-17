using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceStationScript : MonoBehaviour 
{
	public float rotationSpeed;
	public Vector3 orbitcenter;
	
	// Update is called once per frame
	void FixedUpdate () 
	{
		transform.RotateAround(orbitcenter, Vector3.up, rotationSpeed);
	}
}
