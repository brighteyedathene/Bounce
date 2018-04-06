using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeLightColour : MonoBehaviour {

    Light light;

    public Color colorA;
    public Color colorB;
    public float frequency;
    float t = 0;
    float dt = 1;

	// Use this for initialization
	void Start () {
        light = GetComponent<Light>();
	}
	
	// Update is called once per frame
	void Update () {
        if(t > 1)
        {
            t = 1;
            dt = -1;
        }
        else if(t <= 0)
        {
            t = 0;
            dt = 1;
        }
        light.color = colorA * t + colorB * (1 - t);

        

        t += Time.deltaTime * dt * frequency;
	}
}
