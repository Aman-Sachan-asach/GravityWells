using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceStationScript : MonoBehaviour 
{
	public Vector3 pos;
	public Vector3 prevPos;

	public float rotationSpeed;
	public float rotation;
	public float prevRotation;

	public Vector3 orbitcenter;
	public float orbitradius;
	float positionalHeight;

	//other GameObjects
	//ObjectGeneratorScript ogs;

	// Use this for initialization
	void Start () 
	{
		//ogs = GameObject.Find("ObjectGenerator").GetComponent<ObjectGeneratorScript> ();
		positionalHeight = transform.position.y;
	}
	
	// Update is called once per frame
	void Update () 
	{
		prevRotation = rotation;
		rotation += rotationSpeed;

		Quaternion rot = Quaternion.Euler(new Vector3(0, rotation, 0));
		GetComponent<Rigidbody>().MoveRotation(rot);

		prevPos = pos;
		pos.x = orbitcenter.x + orbitradius * Mathf.Sin(rotation * Mathf.Deg2Rad);
		pos.z = orbitcenter.z + orbitradius * Mathf.Cos(rotation * Mathf.Deg2Rad);
		pos.y = positionalHeight;

		/*
		prevPos.x = orbitcenter.x + orbitradius * Mathf.Sin(prevRotation * Mathf.Deg2Rad);
		prevPos.z = orbitcenter.z + orbitradius * Mathf.Cos(prevRotation * Mathf.Deg2Rad);
		prevPos.y = positionalHeight;
		*/

		//rot = Quaternion.FromToRotation(prevPos - orbitcenter, pos - orbitcenter);
		transform.RotateAround(orbitcenter, Vector3.up, rotationSpeed);
	}
}
