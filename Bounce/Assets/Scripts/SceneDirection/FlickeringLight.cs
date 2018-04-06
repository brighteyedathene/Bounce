using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlickeringLight : MonoBehaviour {

    public Light light;
    private float toggleTimer = 0;
    private bool toggle = true;
    private float onIntensity;

	// Use this for initialization
	void Start () {
        light = GetComponentInChildren<Light>();
        onIntensity = light.intensity;
	}
	
	// Update is called once per frame
	void Update () {
        if (toggleTimer < 0)
        {
            light.intensity = (toggle) ? onIntensity : 0;
            toggle = !toggle;
            toggleTimer = Random.Range(0.1f, 2);
        }
        toggleTimer -= Time.deltaTime;
	}
}
