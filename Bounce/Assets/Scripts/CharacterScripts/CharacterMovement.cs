using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour {

    public float RotationAssist;

    public bool disable = false;

    private string verticalAxis = "Vertical";
    private string horizontalAxis = "Horizontal";

    Animator anim;

	// Use this for initialization
	void Start () {
        anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {

        if (disable)
        {
            anim.SetFloat("Forward", 0);
            anim.SetFloat("Right", 0);
        }
        Vector2 movementInput = new Vector2(Input.GetAxis(horizontalAxis), Input.GetAxis(verticalAxis));


        Move(movementInput);
        
	}

    private void Move(Vector2 movementInput)
    {
        // Convert h and v to Forward and Right values for animator

        float intensity = Mathf.Min(1, movementInput.magnitude);

        Vector2 forwardVector = new Vector2(transform.forward.x, transform.forward.z);
        float forward = (movementInput - (movementInput.normalized - forwardVector.normalized)).magnitude * intensity;

        if (intensity > 0.01f)
        {
            Vector2 rightVector = new Vector2(transform.right.x, transform.right.z);
            float right = Vector2.Dot(rightVector.normalized, movementInput.normalized);
            
            float angle = Vector2.Angle(forwardVector, movementInput);
            if (right < 0)
                angle *= -1;

            anim.SetFloat("Right", Mathf.Lerp(anim.GetFloat("Right"),      angle/90, 0.1f));
            anim.SetFloat("Forward", Mathf.Lerp(anim.GetFloat("Forward"), intensity - Mathf.Abs(angle/270), 0.1f));

            // Assist turning
            //transform.Rotate(Vector3.up, Mathf.Max(RotationAssist, Mathf.Abs(angle)) * Mathf.Sign(angle) * Time.deltaTime);
        }
        else
        {
            float clippedRight = Mathf.Lerp(anim.GetFloat("Right"), 0, 0.1f);
            if (Mathf.Abs(clippedRight) < 0.001f)
            {
                clippedRight = 0;
            }

            anim.SetFloat("Right", clippedRight);
            //anim.SetFloat("Right", 0);

            float clippedforward = Mathf.Lerp(anim.GetFloat("Forward"), intensity, 0.1f);
            if (Mathf.Abs(clippedforward) < 0.001f)
            {
                clippedforward = 0;
            }

            anim.SetFloat("Forward", clippedforward);
            //anim.SetFloat("Forward", Mathf.Lerp(anim.GetFloat("Forward"), intensity, 0.2f));
        }



    }

}
