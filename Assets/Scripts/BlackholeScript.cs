using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackholeScript : MonoBehaviour 
{
	public float outerRadius;
	public float innerRadius;
	public float strength;
	public bool flag_InfluencingShip = false;
	public bool flag_CWRotation = false; //this tells us which direction the blackhole is spinning in

	// Use this for initialization
	void Start () 
	{}

	public Vector3 applyForces(ref Vector3 shipPos, float shipMass, float timestep)
	{
		Vector3 r = shipPos - transform.position;

		Vector3 rdash = new Vector3(r.x - innerRadius, r.y - innerRadius, r.z - innerRadius);
		Vector3 temp = new Vector3 (outerRadius - rdash.x, outerRadius - rdash.y, outerRadius - rdash.z);

		Vector3 pullVelocity = new Vector3 (0.0f, 0.0f, 0.0f);
		pullVelocity.x = (-r.x / r.sqrMagnitude) * (temp.x / outerRadius) * (strength * innerRadius);
		pullVelocity.y = (-r.y / r.sqrMagnitude) * (temp.y / outerRadius) * (strength * innerRadius);
		pullVelocity.z = (-r.z / r.sqrMagnitude) * (temp.z / outerRadius) * (strength * innerRadius);
		Vector3 pullForce = pullVelocity * (shipMass/timestep);

		return pullForce;
	}

	public Vector3 applyForcesUsingMomentum(ref Vector3 shipPos, ref Vector3 velocity, float dT)
	{
		float R = 2.75f;
		float GM = 100.47f;

		Vector3 force = -(GM / shipPos.sqrMagnitude) * shipPos.normalized; // + totalForce;
		
		/*
		#update momentum
		sc.p=sc.p+F*dt
		*/

		return force;
	}

	// Update is called once per frame
	void Update () 
	{}
}
