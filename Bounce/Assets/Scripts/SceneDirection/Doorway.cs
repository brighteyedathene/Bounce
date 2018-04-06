using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Doorway : MonoBehaviour {

    public Door leftDoor;
    public Door rightDoor;
    public bool open = false;

    public float openSpeed;
    public float closeSpeed;
    public float openX;
    public float closedX;
    public float currentX;

	// Use this for initialization
	void Start () {
        currentX = closedX;
	}
	
    public void Open()
    {
        StopCoroutine("CloseCoroutine");
        StartCoroutine("OpenCoroutine");
            
    }

    public void Close()
    {
        StopCoroutine("OpenCoroutine");
        StartCoroutine("CloseCoroutine");
    }

    IEnumerator OpenCoroutine()
    {
        Vector3 old = leftDoor.transform.localPosition;
        while (currentX <= openX)
        {
            //currentX = Mathf.Lerp(currentX, openX, openSpeed);
            currentX += Time.deltaTime * openSpeed;

        
            leftDoor.transform.localPosition = new Vector3(-currentX, old.y, old.z);
            rightDoor.transform.localPosition = new Vector3(currentX, old.y, old.z);
            yield return null;
        }
        leftDoor.transform.localPosition = new Vector3(-openX, old.y, old.z);
        rightDoor.transform.localPosition = new Vector3(openX, old.y, old.z);
        open = true;
    }

    IEnumerator CloseCoroutine()
    {
        Vector3 old = leftDoor.transform.localPosition;
        while (currentX >= closedX)
        {
            //currentX = Mathf.Lerp(currentX, closedX, closeSpeed);
            currentX -= Time.deltaTime * closeSpeed;

            
            leftDoor.transform.localPosition = new Vector3(-currentX, old.y, old.z);
            rightDoor.transform.localPosition = new Vector3(currentX, old.y, old.z);
            yield return null;
        }
        leftDoor.transform.localPosition = new Vector3(-closedX, old.y, old.z);
        rightDoor.transform.localPosition = new Vector3(closedX, old.y, old.z);
        open = false;
    }

}
