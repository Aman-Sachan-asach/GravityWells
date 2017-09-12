using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipScript : MonoBehaviour 
{
	public Vector3 position;
	public Vector3 velocity;
	public float fuel;
	public float mass;
	public float drag;
	float positionalHeight;

	public Vector3 force; //the force applied every timestep to the ship that will produce some change in velocity every timestep

	public float dT = 0.02f;
	public Vector3 minVelocity;
	public Vector3 maxVelocity;

	//for docking movement
	public float prevRotation;
	public float dockingRotationSpeed;
	public Vector3 orbitcenter;

	//for dynamic control of ship
	public float thrusterForceMagnitude;
	public float rotationSpeed;
	public float rotation;

	//for RK2 integration
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

	//other GameObjects
	ObjectGeneratorScript ogs;
	GameObject spaceStation;

	//flags
	bool flag_EnteredBlackholeType1 = false;
	bool flag_dockingShip = false;
	bool flag_stopPhysics = false;

	// Use this for initialization
	void Start () 
	{
		ogs = GameObject.Find("ObjectGenerator").GetComponent<ObjectGeneratorScript> ();

		positionalHeight = transform.position.y;
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
			//Attach Ship to SpaceStation
			spaceStation = collider.gameObject;

			flag_stopPhysics = true;
			flag_dockingShip = true;

			//move ship 8.47 units along the spacestations negated up vector
			Vector3 newShipPos = spaceStation.transform.position - 4.22f * spaceStation.transform.up;
			transform.position = newShipPos;

			velocity.x = 0.0f;
			velocity.y = 0.0f;
			velocity.z = 0.0f;

			orbitcenter = spaceStation.GetComponent<SpaceStationScript> ().orbitcenter;
			dockingRotationSpeed = spaceStation.GetComponent<SpaceStationScript> ().rotationSpeed;
		}
		else if(collider.CompareTag("Planet"))
		{
			//Make Ship Explode
			Destroy (gameObject);
			//create an explosion effect here
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
			//Give Boost to ship so it can relaunch

			//continue using physics
			flag_stopPhysics = false;
		}
		else
		{
			Debug.Log("Collided with " + collider.tag);
		}
	}

	void DockShip ()
	{
		//rotate the ship just like the spaceStation
		/*
		prevRotation = rotation;
		rotation -= dockingRotationSpeed;

		Quaternion rot = Quaternion.Euler(new Vector3(0, rotation, 0));
		GetComponent<Rigidbody>().MoveRotation(rot);
		*/
		transform.RotateAround(orbitcenter, Vector3.up, rotationSpeed);
	}

	void PlayerInput()
	{
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
		if ((Input.GetAxisRaw("Vertical") > 0) && !flag_dockingShip)
		{
			//Should use more fuel than rotation
			force += thrusterForceMagnitude*transform.forward;
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
				force += bhs.applyForces (ref position, ref velocity, mass, dT);
			}
		}

		velocity += (force/mass)*dT;
		velocity = velocity * (1.0f - drag);

		//clamp velocity
		velocity.x = Mathf.Clamp(velocity.x, minVelocity.x, maxVelocity.x);
		velocity.y = Mathf.Clamp(velocity.y, minVelocity.y, maxVelocity.y);
		velocity.z = Mathf.Clamp(velocity.z, minVelocity.z, maxVelocity.z);
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

		PlayerInput(); //unity's rigidbody stuff doesnt give you enough control over the physics

		if (!flag_stopPhysics) 
		{
			UpdateVelocity(); //velocity change due to blackholes and other objects in game
			RK2();

			//update position
			gameObject.transform.position = position;
		}

		if (flag_dockingShip) 
		{
			DockShip ();
		}

		//fix height
		position.y = positionalHeight;
    }
}
