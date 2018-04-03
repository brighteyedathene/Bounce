using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Killable : MonoBehaviour {

    private Animator anim;

	// Use this for initialization
	void Start () {
        anim = GetComponent<Animator>();
	}
	
	public void Kill()
    {
        anim.SetBool("Dead", true);
    }
}
