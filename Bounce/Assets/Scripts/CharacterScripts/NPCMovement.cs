using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCMovement : MonoBehaviour {

    private Animator anim;
    public NearbyAgentsDetector detector;

    public float turnSpeed;

    public float blobAvoidanceMultiplier;
    public float harmlessAvoidanceMultiplier;

   

	void Start () {
        anim = GetComponent<Animator>();
	}
	
	void Update () {
        
        // Turn to avoid nearby things
        Evade();
	}

    void Evade()
    {
        // sum up relative directions from nearby blobs, then maybe from other nearby guys
        // then normalize this into a direction and turn to face it a and run
        // or, if it's zero, do nothing

        Vector3 evadeSum = Vector3.zero;
        foreach(GameObject agent in detector.nearbyAgents)
        {
            Vector3 evade = transform.position - agent.transform.position;
            if (agent.CompareTag("Bouncer"))
            {
                evade *= blobAvoidanceMultiplier;
            }
            else
            {
                evade *= harmlessAvoidanceMultiplier;
            }
            evadeSum += evade / evade.magnitude * evade.magnitude;
        }

        if (evadeSum.magnitude > 3)
        {

            Debug.DrawLine(transform.position, transform.position + evadeSum, Color.red);
            Debug.DrawLine(transform.position, transform.position + evadeSum.normalized * 3, Color.yellow);
            Vector3 evadeDir = evadeSum.normalized;

            float angle = Vector3.Angle(evadeDir, transform.forward);
            angle *= Mathf.Sign(Vector3.Dot(evadeDir, transform.right));
            transform.Rotate(Vector3.up, Mathf.Lerp(0, angle, turnSpeed));

            float speed = Mathf.Lerp(anim.GetFloat("Forward"), 1, 0.1f);
            anim.SetFloat("Forward", speed);
        }
        else
        {

            Debug.DrawLine(transform.position, transform.position + evadeSum.normalized * 3, Color.yellow);
            Debug.DrawLine(transform.position, transform.position + evadeSum, Color.red);
            float speed = Mathf.Lerp(anim.GetFloat("Forward"), 0, 0.1f);
            anim.SetFloat("Forward", speed);
        }

    }
}
