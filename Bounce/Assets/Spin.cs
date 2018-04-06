using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spin : MonoBehaviour {

    public float rotationSpeed;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        //Quaternion rotation = Quaternion.Euler(0, rotationSpeed * Time.deltaTime, 0);
        transform.Rotate(Vector3.right * rotationSpeed * Time.deltaTime);
    }
}
