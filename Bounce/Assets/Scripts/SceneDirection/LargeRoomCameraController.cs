using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LargeRoomCameraController : MonoBehaviour {

    public Camera cam;

    public Transform player;
    CharacterMovement playerControl;

    public BouncerMovement bouncer;

    public Doorway doorway;
    public Doorway previousDoorway;

    public SpotloghtController spotcon;

    public float timeBeforeSceneStarts;

    public float doorCloseShotDuration;
    public Transform doorCloseShotPosition;

    public float spotlightShotDuration;
    public Transform spotlightShotPosition;

    public float exitShotDuration;
    public BezierSpline exitShotSpline;

    public float bouncerShotDuration;
    public BezierSpline bouncerShotSpline;

    public Transform finalCamPosition;

    float currentTime = 0;
    bool subevent = false;
    bool active = false;
    public bool sceneWatched = false;

    // Use this for initialization
    void Start()
    {
        playerControl = player.GetComponent<CharacterMovement>();
    }


    IEnumerator PlayScene()
    {
        while(currentTime < timeBeforeSceneStarts)
        {
            currentTime += Time.deltaTime;
            yield return null;
        }

        playerControl.disable = true;

        // Snap to door closing
        currentTime = 0;
        cam.transform.position = doorCloseShotPosition.position;
        cam.transform.rotation = doorCloseShotPosition.rotation;
        while (currentTime < doorCloseShotDuration)
        {
            if (currentTime > doorCloseShotDuration / 3 && !subevent)
            {
                previousDoorway.Close();
                subevent = true;
            }

            currentTime += Time.deltaTime;
            yield return null;
        }

        // Show spotlight
        currentTime = 0;
        subevent = false;
        cam.transform.position = spotlightShotPosition.position;
        cam.transform.rotation = spotlightShotPosition.rotation;
        while (currentTime < spotlightShotDuration)
        {
            if (currentTime > spotlightShotDuration / 4 && !subevent)
            {
                spotcon.ActivateAndTrack(player);
                subevent = true;
            }

            currentTime += Time.deltaTime;
            yield return null;
        }

        // Open the exit door and slowly zoom
        currentTime = 0;
        subevent = false;
        while (currentTime < exitShotDuration)
        {
            if (currentTime > exitShotDuration / 4 && !subevent)
            {
                subevent = true;
                doorway.Open();
            }

            cam.transform.position = exitShotSpline.GetPoint(currentTime / exitShotDuration);
            cam.transform.LookAt(doorway.transform.position + Vector3.up * 2);
            currentTime += Time.deltaTime;
            yield return null;
        }
        sceneWatched = true;
        StartCoroutine("ContinueScene");
    }

    IEnumerator ContinueScene()
    {
        // Continue watching from above while the scene is active
        playerControl.disable = false;
        subevent = false;
        while (active)
        {
            //if (subevent) show the blob turning around
            if(!subevent && bouncer.target == player.transform)
            {
                subevent = true;
                print("Triggered");
                currentTime = 0;
            }
            if (subevent && currentTime < bouncerShotDuration)
            {
                float t = currentTime / bouncerShotDuration;
                t = Mathf.Clamp01(t * t);
                cam.transform.position = bouncerShotSpline.GetPoint(t);

                t = Mathf.Clamp01(t * 3);
                cam.transform.LookAt((doorway.transform.position * 0.3f + player.position * 0.7f) * (1- t) + (bouncer.transform.position +Vector3.up*2) * t);

                Debug.DrawRay(cam.transform.position, cam.transform.forward, Color.magenta);
                currentTime += Time.deltaTime;
            }
            else
            {
                cam.transform.position = finalCamPosition.position;
                cam.transform.LookAt(doorway.transform.position * 0.3f + player.position * 0.7f);
            }
            yield return null;
        }
        spotcon.Deactivate();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            active = true;
            if (!sceneWatched)
                StartCoroutine("PlayScene");
            else
                StartCoroutine("ContinueScene");
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            active = false;
        }
    }
}
