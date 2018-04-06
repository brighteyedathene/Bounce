using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncerContainer : MonoBehaviour {

    public List<BouncerMovement> bouncers;

	// Use this for initialization
	void Start () {
        //gameObject.SetActive(false);
	}
	

    public void ActivateBouncers()
    {
        StartCoroutine("DropBouncers");
        bouncers[0].gameObject.SetActive(true);
        bouncers[1].gameObject.SetActive(true);
    }

    IEnumerator DropBouncers()
    {
        foreach (BouncerMovement bouncer in bouncers)
        {
            bouncer.gameObject.SetActive(true);
            float wait_time = Random.Range(0, 4);
            yield return new WaitForSeconds(wait_time);
        }
    }
}
