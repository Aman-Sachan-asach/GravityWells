using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackholeScript : MonoBehaviour 
{
	public float outerRadius;
	float middle1Radius;
	float middle2Radius;
	float middle3Radius;
	public float innerRadius;
	public float strength;
	public bool flag_InfluencingShip = false;
	public bool flag_CWRotation = false; //this tells us which direction the blackhole is spinning in
	float orbitalSpeed = 0.0f;
	Vector3 orbitalAcceleration = new Vector3(0.0f,0.0f,0.0f);
	float epsilon = 0.5f;
	// Use this for initialization
	void Start () 
	{
		middle1Radius = outerRadius*0.33f;
		middle2Radius = outerRadius*0.5f;
		middle3Radius = outerRadius*0.75f;
	}

	void OnDrawGizmos()
	{
		Gizmos.color = Color.white;
		Gizmos.DrawWireSphere(transform.position, outerRadius);

		Gizmos.color = Color.blue; //middle layer 1
		Gizmos.DrawWireSphere(transform.position, middle1Radius);

		Gizmos.color = Color.green; //middle layer 2
		Gizmos.DrawWireSphere(transform.position, middle2Radius);

		Gizmos.color = Color.cyan; //middle layer 3
		Gizmos.DrawWireSphere(transform.position, middle3Radius);

		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(transform.position, innerRadius);
	}

	public Vector3 applyForces(ref Vector3 shipPos, ref Vector3 shipVel, float shipMass, float timestep)
	{
		Vector3 r = shipPos - transform.position;
		float actualStrength = strength;

		Vector3 rdash = new Vector3(r.x - innerRadius, r.y - innerRadius, r.z - innerRadius);
		Vector3 temp = new Vector3 (outerRadius - rdash.x, outerRadius - rdash.y, outerRadius - rdash.z);

		Vector3 pullVelocity = new Vector3 (0.0f, 0.0f, 0.0f);
		Vector3 pullForce = new Vector3 (0.0f, 0.0f, 0.0f);

		if (r.magnitude < innerRadius) {
			//destroyship byt sucking it in the blackhole
		}
		else if (r.magnitude < middle1Radius) {
			actualStrength *= 2.0f;

			pullVelocity.x = (-r.x / r.sqrMagnitude) * (temp.x / outerRadius) * (actualStrength * innerRadius);
			pullVelocity.y = (-r.y / r.sqrMagnitude) * (temp.y / outerRadius) * (actualStrength * innerRadius);
			pullVelocity.z = (-r.z / r.sqrMagnitude) * (temp.z / outerRadius) * (actualStrength * innerRadius);
			pullForce = pullVelocity * (shipMass/timestep);
		}
		else if (r.magnitude < middle2Radius) {
			actualStrength *= 1.6f;

			pullVelocity.x = (-r.x / r.sqrMagnitude) * (temp.x / outerRadius) * (actualStrength * innerRadius);
			pullVelocity.y = (-r.y / r.sqrMagnitude) * (temp.y / outerRadius) * (actualStrength * innerRadius);
			pullVelocity.z = (-r.z / r.sqrMagnitude) * (temp.z / outerRadius) * (actualStrength * innerRadius);
			pullForce = pullVelocity * (shipMass/timestep);
		}
		else if (r.magnitude < middle3Radius) {
			actualStrength *= 1.2f;

			//change velocity such that it locks to a stable orbit
			if((orbitalSpeed < (shipVel.magnitude + epsilon)) || (orbitalSpeed > (shipVel.magnitude + epsilon)))
			{
				orbitalSpeed = shipVel.magnitude;
				orbitalAcceleration = -r.normalized*(orbitalSpeed*orbitalSpeed/r.magnitude);
			}

			pullForce = orbitalAcceleration * shipMass;
		}
		else{
			pullVelocity.x = (-r.x / r.sqrMagnitude) * (temp.x / outerRadius) * (actualStrength * innerRadius);
			pullVelocity.y = (-r.y / r.sqrMagnitude) * (temp.y / outerRadius) * (actualStrength * innerRadius);
			pullVelocity.z = (-r.z / r.sqrMagnitude) * (temp.z / outerRadius) * (actualStrength * innerRadius);
			pullForce = pullVelocity * (shipMass/timestep);
		}

		return pullForce;
	}

	// Update is called once per frame
	void Update () 
	{}
}
