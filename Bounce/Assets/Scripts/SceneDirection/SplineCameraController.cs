using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplineCameraController : MonoBehaviour {

    public BezierSpline spline;
    public Camera cam;

    public Transform tracking;
    public Transform goal;
    public float maxDistance;
    public float arrivalThreshold;

    public bool active = false;

	// Use this for initialization
	void Start () {
		
	}

    // Update is called once per frame
    void Update() {
        if (!active)
            return;

        float distance = Vector3.Distance(tracking.position, goal.position);

        float current = Mathf.Max(0, distance - arrivalThreshold);
        float total = maxDistance - arrivalThreshold;
        float t = Mathf.Min(1, (total - current) / total);

        cam.transform.position = spline.GetPoint(t);
        cam.transform.LookAt(tracking.position);

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            active = false;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            active = true;
        }
    }
}
