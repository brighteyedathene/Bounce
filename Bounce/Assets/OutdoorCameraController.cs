using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutdoorCameraController : MonoBehaviour {

    public Camera cam;
    public Transform target;

    public BouncerContainer bouncerCon;
    
    public BezierSpline introSpline;
    public float introDuration;

    public BezierSpline outroSpline;
    public BezierSpline outroSplineTarget;
    public float outroDuration;

    public Transform FinalCameraPosition;


    float currentTime = 0;
    bool active = false;

    // Use this for initialization
    void Start()
    {
    }


    IEnumerator PlayScene()
    {
        active = true;
        currentTime = 0;
        while (currentTime < introDuration)
        {
            float t = currentTime / introDuration;
            t = Mathf.Clamp01(t * t);
            cam.transform.position = introSpline.GetPoint(t);
            cam.transform.LookAt(outroSplineTarget.GetPoint(0));

            if (currentTime > introDuration / 3)
            {
            }

            currentTime += Time.deltaTime;
            yield return null;
        }
        bouncerCon.ActivateBouncers();
        currentTime = 0;
        while (currentTime < outroDuration)
        {
            float t = currentTime / outroDuration;
            cam.transform.position = outroSpline.GetPoint(t);
            cam.transform.LookAt(outroSplineTarget.GetPoint(t));

            currentTime += Time.deltaTime;
            yield return null;
        }

        cam.transform.position = FinalCameraPosition.position;
        cam.transform.LookAt(FinalCameraPosition.position + FinalCameraPosition.forward);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!active)
                StartCoroutine("PlayScene");
        }
    }

}
