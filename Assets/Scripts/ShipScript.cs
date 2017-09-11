using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipScript : MonoBehaviour 
{
	public Vector3 position;
	public Vector3 velocity;
	public float fuel;
	public float mass;

	public Vector3 force; //the force applied every timestep to the ship that will produce some change in velocity every timestep

	Vector3 state_pos;
	Vector3 state_vel;
	Vector3 state_force;
	Vector3 state_dot_pos;
	Vector3 state_dot_vel;
	Vector3 state_dot_force;
	Vector3 state_predict_pos;
	Vector3 state_predict_vel;
	Vector3 state_predict_force;
	Vector3 state_predict_dot_pos;
	Vector3 state_predict_dot_vel;
	Vector3 state_predict_dot_force;

	public float dT = 0.02f;
	public Vector3 minVelocity;
	public Vector3 maxVelocity;

	//for dynamic control of ship
	public Vector3 thrusterForce;
	public float thrusterForceMagnitude;
	public float rotationSpeed;
	public float rotation;

	//other GameObjects
	ObjectGeneratorScript ogs;

	//flags
	bool flag_EnteredBlackholeType1 = false;

	// Use this for initialization
	void Start () 
	{
		ogs = GameObject.Find("ObjectGenerator").GetComponent<ObjectGeneratorScript> ();
	}

	void Die () 
	{
		Destroy (gameObject); //removes gameobject from scene and marks it for deletion
	}

	void OnTriggerEnter(Collider collider) //Handles all object interactions
	{
		if(collider.CompareTag("BlackholeType1"))
		{
			collider.gameObject.GetComponent<BlackholeScript>().flag_InfluencingShip = true;
		}
		else if(collider.CompareTag("SpaceStation"))
		{
			//Make Ship Explode
		}
		else if(collider.CompareTag("Planet"))
		{
			//Make Ship Explode
		}
		else if(collider.CompareTag("Moon"))
		{
			//Win condition
		}
		else
		{
			Debug.Log("Collided with " + collider.tag);
		}
	}

	void OnTriggerExit(Collider collider) //Handles all object interactions
	{
		if(collider.CompareTag("BlackholeType1"))
		{
			collider.gameObject.GetComponent<BlackholeScript>().flag_InfluencingShip = false;
		}
		else if(collider.CompareTag("SpaceStation"))
		{
			//Attach Ship to SpaceStation
		}
		else
		{
			Debug.Log("Collided with " + collider.tag);
		}
	}

	// Update is called once per frame
	void Update () 
	{}

	void PlayerInput()
	{
		//CHANGE TO NOT USE RIGIDBODY STUFF FOR MOVEMENT and GRAVITATIONAL stuff

		//Getting current rotation if any
		if (Input.GetAxisRaw("Horizontal") > 0)
		{
			//uses barely any fuel
			rotation += rotationSpeed;
			Quaternion rot = Quaternion.Euler(new Vector3(0, rotation-90.0f, 0));
			GetComponent<Rigidbody>().MoveRotation(rot);
		}
		else if (Input.GetAxisRaw("Horizontal") < 0)
		{
			//uses barely any fuel
			rotation -= rotationSpeed;
			Quaternion rot = Quaternion.Euler(new Vector3(0, rotation-90.0f, 0));
			GetComponent<Rigidbody>().MoveRotation(rot);
		}

		//regular movement based on how much fuel the ship has left
		//force Thruster
		if (Input.GetAxisRaw("Vertical") > 0)
		{
			//Should use more fuel than rotation
			//GetComponent<Rigidbody>().AddRelativeForce(thrusterForce);
			Vector3 worldSpaceLookAtVec =  transform.forward;
			//Debug.DrawLine(position, position+worldSpaceLookAtVec*10, Color.blue, 2.0f);
			force += thrusterForceMagnitude*worldSpaceLookAtVec;
		}
	}

	void UpdateVelocity()
	{
		//Change Velocity and position based on the blackholes gravitational pull
		List<GameObject> blackholes = ogs.blackholes;
		for (int i = 0; i < blackholes.Count; i++) 
		{
			BlackholeScript bhs = ogs.blackholes [i].GetComponent<BlackholeScript> ();

			if (bhs.flag_InfluencingShip) 
			{
				force += bhs.applyForces (ref position, mass, dT);
				//force += bhs.applyForcesUsingMomentum(ref position, ref velocity, dT);
			}
		}

		velocity += (force/mass)*dT;
	}
    
	void computeDerivative(ref Vector3 statePos, ref Vector3 stateVel, ref Vector3 stateForce,
		ref Vector3 stateDotPos, ref Vector3 stateDotVel, ref Vector3 stateDotForce)
	{
		stateDotPos = stateVel;
		stateDotVel = stateForce / mass;
		stateDotForce.x = 0.0f;
		stateDotForce.y = 0.0f;
		stateDotForce.z = 0.0f;
	}

	void RK2()
	{
		state_pos = transform.position;
		state_vel = velocity;
		state_force = force;

		computeDerivative(ref state_pos, ref state_vel, ref state_force, ref state_dot_pos, ref state_dot_vel, ref state_dot_force);

		state_predict_pos = state_pos + state_dot_pos * dT;
		state_predict_vel = state_vel + state_dot_vel * dT;
		state_predict_force = state_force + state_dot_force * dT;

		computeDerivative(ref state_predict_pos, ref state_predict_vel, ref state_predict_force, 
			ref state_predict_dot_pos, ref state_predict_dot_vel, ref state_predict_dot_force);

		state_pos = state_pos + (dT / 2.0f) * (state_dot_pos + state_predict_dot_pos);
		state_vel = state_vel + (dT / 2.0f) * (state_dot_vel + state_predict_dot_vel);
		state_force = state_force + (dT / 2.0f) * (state_dot_force + state_predict_dot_force);

		position = state_pos;
		velocity = state_vel;
	}

	void resetValuesEveryTimestep()
	{
		force.x = 0.0f;
		force.y = 0.0f;
		force.z = 0.0f;

		state_predict_pos.x = 0.0f;
		state_predict_pos.y = 0.0f;
		state_predict_pos.z = 0.0f;

		state_predict_vel.x = 0.0f;
		state_predict_vel.y = 0.0f;
		state_predict_vel.z = 0.0f;

		state_predict_force.x = 0.0f;
		state_predict_force.y = 0.0f;
		state_predict_force.z = 0.0f;

		state_predict_dot_pos.x = 0.0f;
		state_predict_dot_pos.y = 0.0f;
		state_predict_dot_pos.z = 0.0f;

		state_predict_dot_vel.x = 0.0f;
		state_predict_dot_vel.y = 0.0f;
		state_predict_dot_vel.z = 0.0f;

		state_predict_dot_force.x = 0.0f;
		state_predict_dot_force.y = 0.0f;
		state_predict_dot_force.z = 0.0f;
	}

	void FixedUpdate () 
	{
		resetValuesEveryTimestep ();

		PlayerInput(); //maybe dont use rigidbody stuff of unity -- mariano's suggestion
		UpdateVelocity(); //velocity change due to blackholes and other objects in game
		RK2();

		//update position
		gameObject.transform.position = position;
    }
}
