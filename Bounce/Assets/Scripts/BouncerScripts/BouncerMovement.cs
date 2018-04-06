using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncerMovement : MonoBehaviour {

    public NearbyAgentsDetector detector;

    public Transform target;
    public float killDistance;
    public float maxAvoidForce;
    public float avoidForceMultiplier;
    private float currentDistanceToTarget = 0;

    public float scaleMax;
    public float scaleFluidity;
    public float scaleExagerration;
    private float baseScale;
    private float currentScale = 0;

    public float jumpTimer = 0;
    public float jumpCooldown;
    public float turnSpeed;
    public float predictionModifier;

    Vector3 currentJumpTarget;
    public float jumpChargeTime;
    public float jumpForwardForce;
    public float jumpUpForce;
    private bool charging = false;

    private float distanceToGround;
    private bool grounded;
    public float groundTestLength;

    Rigidbody body;
    BouncerMeshHandle meshHandle;
    SpringHandle springHandle;
    SpringAnchor springAnchor;

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


        if (Physics.Raycast(transform.position, -Vector3.up, groundTestLength, 1 << LayerMask.NameToLayer("FloorLayer")))
        {
            if (!charging)
            {
                jumpTimer += Time.deltaTime;
                if (jumpTimer > jumpCooldown)
                {
                    target = FindNearestTargetTransform();
                    if (target)
                    {
                        currentJumpTarget = target.position;
                        StartCoroutine("JumpCoroutine");
                        jumpTimer = 0;
                    }
                }
            }
        }

        UpdateScale();
        CheckForKillsAndCollisions();
	}

    IEnumerator JumpCoroutine()
    {
        charging = true;
        float charge = 0;
        while(charge < jumpChargeTime)
        {
            Turn();
            charge += Time.deltaTime;
            springHandle.transform.Translate(Vector3.down * charge * 0.1f);

            yield return null;
        }
        charging = false;
        Jump();
    }

    void Jump()
    {
        // Jump range is about 25 with 500 horizontal, 600 vertical jump force
        float forwardEffort = Mathf.Min(1, currentDistanceToTarget / 25);
        body.AddForce(transform.forward * jumpForwardForce * forwardEffort + transform.up * jumpUpForce);
        //print("Jump!");
    }

    void Turn()
    {
        Rigidbody targetBody = target.GetComponent<Rigidbody>();
        if (targetBody)
        {
            currentJumpTarget = currentJumpTarget * (1 - predictionModifier) + (target.position + targetBody.velocity * 1.5f) * predictionModifier;
        }
        Vector3 targetDirection = currentJumpTarget - transform.position;
        Debug.DrawLine(target.position + Vector3.up, currentJumpTarget + Vector3.up, Color.green);

        currentDistanceToTarget = (Vector3.Distance(transform.position, currentJumpTarget));

        float angle = Vector3.Angle(transform.forward, targetDirection);
        angle *= Mathf.Sign(Vector3.Dot(transform.right, targetDirection));
        transform.Rotate(Vector3.up, angle * Time.deltaTime * turnSpeed);
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

    void CheckForKillsAndCollisions()
    {
        Vector3 avoidVector = Vector3.zero;
        foreach(GameObject agent in detector.nearbyAgents)
        {
            Killable killable = agent.GetComponent<Killable>();
            if (killable)
            {
                // Draw rays to debug
                Vector3 raystart = transform.position + Vector3.up * 5;
                Vector3 direction = agent.transform.position + Vector3.up * 2 - raystart;
                Debug.DrawRay(raystart, direction, Color.green);
                Debug.DrawRay(raystart, direction.normalized * killDistance, Color.blue);
                //print("distance:  " + direction.magnitude);

                // Distance to target's head or thereabuots
                if (Vector3.Distance(transform.position + Vector3.up * 2, agent.transform.position) < killDistance)
                {
                    killable.Kill();
                }
            }
            else
            {
                Vector3 raystart = transform.position + Vector3.up * 5;
                Vector3 direction = agent.transform.position + Vector3.up * 5 - raystart;
                Debug.DrawRay(raystart, direction, Color.green);
                Debug.DrawRay(raystart, direction.normalized * killDistance, Color.magenta);

                if (Vector3.Distance(transform.position + Vector3.up * 5, agent.transform.position) < killDistance)
                {
                    avoidVector += direction.normalized * avoidForceMultiplier / direction.magnitude;
                }
            }

        }

        // apply aviod force
        if (avoidVector.magnitude > maxAvoidForce)
        {
            avoidVector = avoidVector.normalized * maxAvoidForce;
        }
        body.AddForce(avoidVector);
    }

    Transform FindNearestTargetTransform()
    {
        float nearest = Mathf.Infinity;
        Transform nearestTarget = null;
        foreach (GameObject agent in detector.nearbyAgents)
        {
            // Make sure it's not dead
            Killable killable = agent.GetComponent<Killable>();
            if (killable)
            {
                if (killable.dead)
                {
                    //detector.nearbyAgents.Remove(agent);
                    continue;
                }

                float distance = Vector3.Distance(transform.position, agent.transform.position);
                if (distance < nearest)
                {
                    nearestTarget = agent.transform;
                    nearest = distance;
                }
            }


        }
        return nearestTarget;
    }
}
