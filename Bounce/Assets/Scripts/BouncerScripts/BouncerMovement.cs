using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncerMovement : MonoBehaviour {

    public CrowdManager cm;

    public Transform target;
    public float killDistance;
    private float currentDistanceToTarget;


    public float scaleMax;
    public float scaleFluidity;
    public float scaleExagerration;
    private float baseScale;
    private float currentScale = 0;

    public Rigidbody body;

    private float jumpTimer = 0;
    public float jumpCooldown;
    public float turnSpeed;

    public float jumpChargeTime;
    public float jumpForwardForce;
    public float jumpUpForce;

    private float distanceToGround;
    private bool grounded;
    public float groundTestLength;

    public BouncerMeshHandle meshHandle;
    public SpringHandle springHandle;
    public SpringAnchor springAnchor;

	// Use this for initialization
	void Start () {
        body = GetComponent<Rigidbody>();
        meshHandle = GetComponentInChildren<BouncerMeshHandle>();
        springHandle = GetComponentInChildren<SpringHandle>();
        springAnchor = GetComponentInChildren<SpringAnchor>();

        baseScale = meshHandle.transform.localScale.y;
	}
	
	// Update is called once per frame
	void Update () {

        // This is used in several functions in the update
        currentDistanceToTarget = (Vector3.Distance(transform.position, target.position));

        if (jumpTimer > jumpCooldown)
        {
            StartCoroutine("JumpCoroutine");
            jumpTimer = 0;
        }
        else if (Physics.Raycast(transform.position, -Vector3.up, groundTestLength))
        {
            jumpTimer += Time.deltaTime;
        }
        else
        {
        }


        UpdateScale();

        CheckForKills();

	}

    IEnumerator JumpCoroutine()
    {
        float charge = 0;
        while(charge < jumpChargeTime)
        {
            Turn();
            charge += Time.deltaTime;
            springHandle.transform.Translate(Vector3.down * charge * 0.1f);

            yield return null;
        }
        Jump();
    }

    void Jump()
    {
        // Jump range is about 15
        float forwardEffort = Mathf.Min(1, currentDistanceToTarget / 25);
        body.AddForce(transform.forward * jumpForwardForce * forwardEffort + transform.up * jumpUpForce);
        print("Jump!");
    }

    void Turn()
    {
        if (currentDistanceToTarget < 2)
        {
            return;
        }
        Vector3 targetDirection = target.position - transform.position;
        float angle = Vector3.Angle(transform.forward, targetDirection);
        if (angle > turnSpeed)
        {
            angle = turnSpeed;
        }
        angle *= Mathf.Sign(Vector3.Dot(transform.right, targetDirection));
        transform.Rotate(Vector3.up, angle * Time.deltaTime);
    }

    void UpdateScale()
    {
        float scaler =  springHandle.transform.position.y - springAnchor.transform.position.y;
        if (body.velocity.y > 0.1)
            scaler *= -body.velocity.y / 12; // usually, the jumps move no faster than 12

        currentScale = Mathf.Min(10, Mathf.Lerp(currentScale, scaler, scaleFluidity));

        float vScale = baseScale + (1 + currentScale) * scaleExagerration;
        float hScale = baseScale + (1 - currentScale) * scaleExagerration;
        meshHandle.transform.localScale = new Vector3(hScale, vScale, hScale);

    }

    void CheckForKills()
    {

        Vector3 direction = target.position + Vector3.up * 3 - transform.position;
        Debug.DrawRay(transform.position, direction, Color.green);
        Debug.DrawRay(transform.position, direction.normalized * killDistance, Color.red);
        print("distance:  " + direction.magnitude);

        // Distance to target's head or thereabuots
        if (Vector3.Distance(transform.position, target.position + Vector3.up * 3) < killDistance)
        {
            Killable killable = target.GetComponent<Killable>();
            if (killable != null)
            {
                killable.Kill();
            }
            else
            {
                print("cuodln't find killable component");
            }
        }
    }
}
