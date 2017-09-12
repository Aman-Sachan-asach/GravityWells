using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceStationScript : MonoBehaviour 
{
	public float rotationSpeed;
	public float rotation;
	public float prevRotation;

	public Vector3 orbitcenter;
	float positionalHeight;

	// Use this for initialization
	void Start () 
	{
		positionalHeight = transform.position.y;
	}
	
	// Update is called once per frame
	void FixedUpdate () 
	{
		transform.RotateAround(orbitcenter, Vector3.up, rotationSpeed);
	}
}
