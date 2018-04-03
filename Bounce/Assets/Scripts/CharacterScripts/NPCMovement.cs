using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCMovement : MonoBehaviour {

    public Transform closestBlob;

    public float turnSpeed;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        // Find closest blob

        // Try to face away from blob
        EvadeBlob();
	}

    void EvadeBlob()
    {
        Vector3 evadeDir = transform.position - closestBlob.position;
        float angle = Vector3.Angle(evadeDir, transform.forward);
        angle *= Mathf.Sign(Vector3.Dot(evadeDir, transform.right));
        transform.Rotate(Vector3.up, Mathf.Lerp(0, angle, turnSpeed));
    }
}
