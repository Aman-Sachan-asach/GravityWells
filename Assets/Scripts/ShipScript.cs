using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class ShipScript : MonoBehaviour 
{
    Vector3 prevPosition;
	public Vector3 position;
	public Vector3 velocity;
    public float mass;
	Vector3 fixPositionalHeight;

    //Fuel
    public float currentFuel;
    public float MaxFuel;
    public float thrustFuelUsed;
    public float rotationFuelUsed;
    //Fuel Bar handle
    Slider fuelBar;

    //Relaunch Speed Multiplier
    public float relaunchSpeed;

	public float dT = 0.02f;
	public Vector3 minVelocity;
	public Vector3 maxVelocity;
    public Vector3 force; //the force applied every timestep to the ship that will produce some change in velocity every timestep

    //for docking movement
    public float dockingRotationSpeed;
	public Vector3 orbitcenter;

	//for dynamic control of ship
	public float thrusterForceMagnitude;
	public float rotationSpeed;

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
    GameObject camera;
    GamePlayManagerScript gms;
    ObjectGeneratorScript ogs;
    public GameObject Explosion;
    //GameObject gameOver;
    GameObject stageCleared;
    GameObject spaceStation;

	//flags
    bool flag_ShipDocked = false;
	bool flag_stopPhysics = false;
    bool flag_stopAcceptingInput = false;

    // Use this for initialization
    void Start () 
	{
		ogs = GameObject.Find("ObjectGenerator").GetComponent<ObjectGeneratorScript> ();
        gms = GameObject.Find("GamePlayManager").GetComponent<GamePlayManagerScript>();
        camera = GameObject.Find("Main Camera");

        fuelBar = gms.fuelBar;

        //set camera so that it starts off behind and above the ship
        Vector3 camVec = transform.position;
        camVec.y += 40.0f;
        camVec += 18 * -transform.forward;
        camera.transform.position = camVec;

        //set fuel levels
        currentFuel = MaxFuel;
        fuelBar.value = MaxFuel;

        fixPositionalHeight = transform.position;

        //for win condition
        stageCleared = gms.stageCleared;
        stageCleared.SetActive(false);
	}

	public void Die () 
	{
		Destroy (gameObject); //removes gameobject from scene and marks it for deletion
	}

    public void DieWithExplosion()
    {
        //Make Ship Explode
        Destroy(gameObject);

        //create an explosion effect here
        Vector3 pos = transform.position;
        GameObject tempExplosion = Instantiate(Explosion, pos, Quaternion.identity) as GameObject;
    }

    void OnTriggerEnter(Collider collider) //Handles all object interactions
	{
		if(collider.CompareTag("BlackholeType1"))
		{
			collider.gameObject.GetComponent<BlackholeScript>().flag_InfluencingShip = true;
		}
        else if (collider.CompareTag("RepulsiveStarType1"))
        {
            collider.gameObject.GetComponent<RepulsiveStarScript>().flag_InfluencingShip = true;
        }
        else if(collider.CompareTag("SpaceStation"))
		{
			//Attach Ship to SpaceStation
			spaceStation = collider.gameObject;

			flag_stopPhysics = true;
			flag_ShipDocked = true;
            
            //move ship 8.47 units along the spacestations negated up vector
            Vector3 newShipPos = spaceStation.transform.position - 4.22f * spaceStation.transform.up;
			transform.position = newShipPos;

            Vector3 dockedOrientation = -spaceStation.transform.up;
            float angle = Vector3.Angle(dockedOrientation, transform.forward);
            transform.Rotate(0, angle,0);

            Debug.DrawLine(transform.position, transform.position + transform.up, Color.white, 2.0f, true);

            velocity.x = 0.0f;
			velocity.y = 0.0f;
			velocity.z = 0.0f;

			orbitcenter = spaceStation.GetComponent<SpaceStationScript> ().orbitcenter;
			dockingRotationSpeed = spaceStation.GetComponent<SpaceStationScript> ().rotationSpeed;
		}
		else if(collider.CompareTag("Planet"))
		{
            DieWithExplosion();
            gms.StartCoroutine("GameOverandReset");
        }
		else if(collider.CompareTag("Moon"))
		{
            //stop moving the ship
            velocity.x = 0.0f;
            velocity.y = 0.0f;
            velocity.z = 0.0f;

            //stop physics
            flag_stopPhysics = true;
            //stop taking inputs
            flag_stopAcceptingInput = true;

            StartCoroutine("ClearedStage");
        }
		else
		{
			Debug.Log("Collided with " + collider.tag);
		}
	}

    public IEnumerator ClearedStage()
    {
        //Win condition
        stageCleared.SetActive(true);

        yield return new WaitForSeconds(3.0f);

        //Generate NextLevel
        gms.NextLevel();
    }

    void OnTriggerExit(Collider collider) //Handles all object interactions
	{
		if(collider.CompareTag("BlackholeType1"))
		{
			collider.gameObject.GetComponent<BlackholeScript>().flag_InfluencingShip = false;
		}
        else if (collider.CompareTag("RepulsiveStarType1"))
        {
            collider.gameObject.GetComponent<RepulsiveStarScript>().flag_InfluencingShip = false;
        }
  //      else
		//{
		//	Debug.Log("Collided with " + collider.tag);
		//}
	}

	void RotateDockedShip ()
	{
		//rotate the ship just like the spaceStation, ie around the blackhole
		transform.RotateAround(orbitcenter, Vector3.up, dockingRotationSpeed);
	}

    void PlayerInput()
    {
        if(!flag_stopAcceptingInput)
        {
            //Getting current rotation if any
            if (Input.GetAxisRaw("Horizontal") > 0)
            {
                //uses barely any fuel
                currentFuel -= rotationFuelUsed;
                fuelBar.value = currentFuel/MaxFuel;

                //rotate
                transform.RotateAround(transform.position, Vector3.up, rotationSpeed);
            }
            else if (Input.GetAxisRaw("Horizontal") < 0)
            {
                //uses barely any fuel
                currentFuel -= rotationFuelUsed;
                fuelBar.value = currentFuel / MaxFuel;

                //rotate
                if (flag_ShipDocked)
                {
                    transform.RotateAround(transform.position, Vector3.up, -2.0f * rotationSpeed);
                }
                else
                {
                    transform.RotateAround(transform.position, Vector3.up, -rotationSpeed);
                }
            }

            //regular movement based on how much fuel the ship has left
            //force Thruster
            if ((Input.GetAxisRaw("Vertical") > 0) && !flag_ShipDocked)
            {
                //Should use more fuel than rotation
                currentFuel -= thrustFuelUsed;
                fuelBar.value = currentFuel / MaxFuel;
                
                //thrust
                force += thrusterForceMagnitude * transform.forward;
            }

            //Relaunch from spacestation
            if (Input.GetButton("Relaunch"))// && flag_ShipDocked)
            {
                flag_stopPhysics = false;
                flag_ShipDocked = false;

                //give velocity boost
                velocity = transform.forward * relaunchSpeed;

                //refill fuel
                currentFuel = MaxFuel;
                fuelBar.value = MaxFuel;
            }
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

        //Change Velocity and position based on the repulsive Stars push
        List<GameObject> repulsiveStars = ogs.repulsiveStars;
        for (int i = 0; i < repulsiveStars.Count; i++)
        {
            RepulsiveStarScript rss = ogs.repulsiveStars[i].GetComponent<RepulsiveStarScript>();

            if (rss.flag_InfluencingShip)
            {
                force += rss.applyForces(ref position, mass, dT);
            }
        }

        velocity += (force/mass)*dT; //no drag

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
        prevPosition = transform.position;
		resetValuesEveryTimestep ();

		PlayerInput(); //unity's rigidbody stuff doesnt give you enough control over the physics

		if (!flag_stopPhysics) 
		{
			UpdateVelocity(); //velocity change due to blackholes and other objects in game
			RK2();

			//update position
			gameObject.transform.position = position;
		}

		if (flag_ShipDocked) 
		{
			RotateDockedShip ();
		}

        //update camera so it follows the ship
        Vector3 translateCamVec = gameObject.transform.position - prevPosition;
        camera.transform.position += translateCamVec;

        //lock height
        fixPositionalHeight.x = transform.position.x;
        fixPositionalHeight.z = transform.position.z;
        transform.position = fixPositionalHeight;
    }
}
