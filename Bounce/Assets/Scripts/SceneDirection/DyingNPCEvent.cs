using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DyingNPCEvent : MonoBehaviour {

    public Animator dyingAnim;
    public SplineCameraController hallwayDirection;
    public Camera cam;
    public Transform target;
    public Doorway doorway;
    public BezierSpline spline;
    public float duration;
    float currentTime = 0;
    bool active = false;

	// Use this for initialization
	void Start () {
        spline = GetComponent<BezierSpline>();
	}
	

    IEnumerator PlayScene()
    {
        hallwayDirection.active = false;
        while(currentTime < duration)
        {
            cam.transform.position = spline.GetPoint(currentTime / duration);
            cam.transform.LookAt(target.position);
            currentTime += Time.deltaTime;

            if (currentTime > duration / 3)
            {
                dyingAnim.SetBool("Dead", true);
            }

            yield return null;
        }
        hallwayDirection.active = true;
        doorway.Open();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine("PlayScene");
        }
    }

}
