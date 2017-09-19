using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepulsiveStarScript : MonoBehaviour
{
    public float outerRadius;
    public float innerRadius;
    public float strength;
    public bool flag_InfluencingShip = false;

    public Vector3 applyForces(ref Vector3 shipPos, float shipMass, float timestep)
    {
        Vector3 r = shipPos - transform.position;

        Vector3 pushVelocity = new Vector3(0.0f, 0.0f, 0.0f);
        Vector3 pushForce = new Vector3(0.0f, 0.0f, 0.0f);

        if (r.magnitude < innerRadius)
        {
            //destroy ship via explosion
            ObjectGeneratorScript obs = GameObject.Find("ObjectGenerator").GetComponent<ObjectGeneratorScript>();
            GamePlayManagerScript gms = GameObject.Find("GamePlayManager").GetComponent<GamePlayManagerScript>();
            ShipScript shipscript = obs.playerShip.GetComponent<ShipScript>();

            shipscript.DieWithExplosion();
            gms.StartCoroutine("GameOverandReset");
        }
        else
        {
            pushVelocity.x = (outerRadius/r.x) * strength;
            pushVelocity.y = (outerRadius/r.y) * strength;
            pushVelocity.z = (outerRadius/r.z) * strength;
            pushForce = pushVelocity * (shipMass / timestep);
        }

        return pushForce;
    }
}
