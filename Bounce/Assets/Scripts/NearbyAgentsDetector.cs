using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NearbyAgentsDetector : MonoBehaviour {

    string NPCTag = "Killable";

    public List<GameObject> nearbyAgents;

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.GetInstanceID() == transform.parent.gameObject.GetInstanceID())
        {
            //print("didn't add " + other.gameObject.ToString() + " because i'm " + transform.parent.gameObject.ToString());
            return;
        }

        Killable agent = other.gameObject.GetComponent<Killable>();
        //if (other.CompareTag(NPCTag))
        if (agent && agent.dead)
        {
            //print("ignoring dead");
        }
        else
        {
            //print("adding a " + other.ToString());
            nearbyAgents.Add(other.gameObject);
        }
    }

    void OnTriggerExit(Collider other)
    {
        //print("removing a " + other.ToString());
        nearbyAgents.Remove(other.gameObject);
    }

}
