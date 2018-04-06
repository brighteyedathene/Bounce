using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Killable : MonoBehaviour {

    private Animator anim;
    public bool dead;

	// Use this for initialization
	void Start () {
        anim = GetComponent<Animator>();
	}

    private void Update()
    {
        anim.SetBool("Dead", dead);
    }

    public void Kill()
    {
        dead = true;
        //anim.SetBool("Dead", true);
    }
}
