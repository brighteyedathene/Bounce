using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpotloghtController : MonoBehaviour {

    Light spotlight;
    float intensity;
    bool on;

    public float latency;

	// Use this for initialization
	void Start () {
        spotlight = GetComponentInChildren<Light>();
        intensity = spotlight.intensity;
        spotlight.intensity = 0;
    }
	
    public void ActivateAndTrack(Transform target)
    {
        on = true;
        StartCoroutine(TrackCoroutine(target));
    }

	IEnumerator TrackCoroutine(Transform target)
    {
        while (on)
        {
            spotlight.intensity = Mathf.Lerp(spotlight.intensity, intensity, latency);

            Vector3 targetPoint = target.position * latency + (transform.position + transform.forward) * (1-latency);
            transform.LookAt(targetPoint);
            yield return null;
        }
    }

    public void Deactivate()
    {
        on = false;
        spotlight.intensity = 0;
    }

    IEnumerator DeactivateCoroutine()
    {
        while (on)
        {
            spotlight.intensity = Mathf.Lerp(spotlight.intensity, 0, latency);

            Vector3 targetPoint = (transform.position - Vector3.up) * latency + (transform.position + transform.forward) * (1 - latency);
            transform.LookAt(targetPoint);
            yield return null;
        }
    }
}
