﻿using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using System.Collections.Generic;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]

public class PlayerController : MonoBehaviour
{
    #region Variables (private)

    private bool grounded = false;
    private Vector3 groundVelocity;
    private CapsuleCollider capsule;

    // Inputs Cache
    private bool jumpFlag = false;
    private bool frozen = false;
    private Vector3 tempVel;

    private Camera camera;
    private Rigidbody rb;
    private Queue<Vector3> oldVelocitys;

    private UnityAction openEyesListener;
    private UnityAction closedEyesListener;

    private float currentHealth;
    public  float currentFP;

    private float tickrate = 0.1f;

    #endregion

    #region Properties (public)

    // Damage
    // this is the velocity floor that will cause dmg to occur 
    public float damageThreshold;

    // Speeds
    public float walkSpeed = 8.0f;
    public float walkBackwardSpeed = 4.0f;
    public float runSpeed = 14.0f;
    public float runBackwardSpeed = 6.0f;
    public float sidestepSpeed = 8.0f;
    public float runSidestepSpeed = 12.0f;
    public float maxVelocityChange = 10.0f;
    public float maxVelocity = 10.0f;

    // Air
    public float inAirControl = 0.1f;
    public float jumpHeight = 2.0f;

    // Can Flags
    public bool canRunSidestep = true;
    public bool canJump = true;
    public bool canRun = true;

    // Camera Control
    public float sensitivityX = 1.0f;
    public float sensitivityY = 1.0f;
    Vector3 rotationX;
    Vector3 rotationY;

    // Player Health
    public float maxHealth = 100f;
    public float maxFP = 100f;
    public float stopFPCost = 25;
    public float FPDrainRate = 3f;
    public float FPRegenRate = 5f;

    float MinClamp = -80f;
    float MaxClamp = 70f;

    #endregion

    #region Unity event functions

    void Awake()
    {
        capsule = GetComponent<CapsuleCollider>();
        GetComponent<Rigidbody>().freezeRotation = true;
        GetComponent<Rigidbody>().useGravity = true;
        openEyesListener = new UnityAction(OpenEyes);
        closedEyesListener = new UnityAction(CloseEyes);
    }

    void OnEnable()
    {
        EventManager.StartListening("Eyes Open", openEyesListener);
        EventManager.StartListening("Eyes Closed", closedEyesListener);
    }

    void OnDisable()
    {
        EventManager.StopListening("Eyes Open", openEyesListener);
        EventManager.StopListening("Eyes Closed", closedEyesListener);
    }

    void Start()
    {
        camera = GetComponentInChildren<Camera>();
        rb = GetComponent<Rigidbody>();
        oldVelocitys = new Queue<Vector3>();
        currentHealth = maxHealth;
        currentFP = maxFP;
        InvokeRepeating("ManageFP", 0f, tickrate);
    }

    void Update()
    {
        // Cache the input
        if (Input.GetButtonDown("Jump") && grounded)
            jumpFlag = true;

        if (Input.GetAxis("Stop") > 0 && !frozen && currentFP > stopFPCost)
        {
            EventManager.TriggerEvent("Eyes Open");
        }
        else if ((Input.GetAxis("Stop") == 0 && frozen) || currentFP <= 0)
        {
            EventManager.TriggerEvent("Eyes Closed");
        }
        if (Input.GetButtonDown("ResetCamera"))
        {
            ResetCamera();
        }
    }

    void LateUpdate()
    {
        // Add the new velocity and get rid of oldest
        oldVelocitys.Enqueue(rb.velocity);
        if(oldVelocitys.Count > 3)
        {
            oldVelocitys.Dequeue();
        }
    }

    /// Update for physics
    void FixedUpdate()
    {
        // Cache de input
        var inputVector = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));


        if (!frozen)
        {
            // On the ground
            if (grounded)
            {
                // Apply a force that attempts to reach our target velocity
                var velocityChange = CalculateVelocityChange(inputVector);
                rb.AddForce(velocityChange, ForceMode.VelocityChange);

                // Jump
                if (canJump && jumpFlag)
                {
                    jumpFlag = false;
                    rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y + CalculateJumpVerticalSpeed(), rb.velocity.z);
                }

                // By setting the grounded to false in every FixedUpdate we avoid
                // checking if the character is not grounded on OnCollisionExit()
                grounded = false;
            }
            // In mid-air
            else
            {
                // Uses the input vector to affect the mid air direction
                var velocityChange = transform.TransformDirection(inputVector)*inAirControl;
                rb.AddForce(velocityChange, ForceMode.VelocityChange);
            }

            //fix this if we need the player to actually go faster than maxVelocity due to force 
            if (rb.velocity.sqrMagnitude > maxVelocity)
            {
                rb.velocity *= 0.98f;
            }
        }

        //Camera related things
        float turnY = Input.GetAxisRaw("CameraHorizontal");
        float turnX = Input.GetAxisRaw("CameraVertical");
        

        RotatePlayer(turnY);
        RotateCamera(turnX);

    }

    // Unparent if we are no longer standing on our parent
    void OnCollisionExit(Collision collision)
    {
        if (collision.transform == transform.parent)
            transform.parent = null;
    }

    // If there are collisions check if the character is grounded
    void OnCollisionStay(Collision col)
    {
        TrackGrounded(col);
    }

    void OnCollisionEnter(Collision col)
    {
        TrackGrounded(col);

        // Calc Fall Damage
        // Tracks the average velocity over the last three frames
        Vector3 avg = Vector3.zero;
        for(int i = 0; i < oldVelocitys.Count; ++i)
        {
            avg += oldVelocitys.ToArray()[i];
        }
        avg = avg / oldVelocitys.Count;
        if((-1*avg.y) > damageThreshold)
        {
            Debug.Log("You have taken fall damage");

        }
    }

    #endregion

    #region Methods

    public void ApplyPlayerDamage(float dmg)
    {
        Debug.Log("ehere");
        currentHealth -= dmg;
        StartCoroutine("damageFlash");
        GetComponent<AudioSource>().Play();
        Debug.Log(currentHealth);

        if (currentHealth <= 0)
        {
            EventManager.TriggerEvent("Player Death");
            Debug.Log("You Have Died");
            
        }
    }

    IEnumerator damageFlash()
    {
        DialateEffect effect = Camera.main.GetComponent<DialateEffect>();
        for (float f = 0f; f <1; f += 0.25f)
        {
            effect.intensity = f;
            yield return new WaitForSeconds(0.025f);
        }
        for (float f = 1f; f > 0; f -= 0.1f)
        {
            effect.intensity = f;
            yield return new WaitForSeconds(.025f);
        }
        effect.intensity = 0;
    }

    private void OpenEyes()
    {
        Freeze();
        currentFP -= stopFPCost;
    }

    private void CloseEyes()
    {
        Freeze();
    }

    private void Freeze()
    {
        frozen = !frozen;

        if (frozen)
        {
            tempVel = rb.velocity;
            rb.constraints = RigidbodyConstraints.FreezePosition;
        }
        else
        {
            rb.constraints = RigidbodyConstraints.FreezeRotation;
            rb.velocity = tempVel;
        }
    }

    private void ManageFP()
    {
        if (frozen)
        {
            currentFP -= FPDrainRate*tickrate;
        }
        else
        {
            currentFP += FPRegenRate*tickrate;
        }
        currentFP = Mathf.Clamp(currentFP, 0, 100);
    }

    private void RotateCamera(float turnX)
    {
        turnX = turnX * sensitivityX;

        Vector3 currentRotation = camera.transform.rotation.eulerAngles;
        currentRotation.x += turnX;
        Quaternion deltaRotation = Quaternion.Euler(currentRotation);
        
        camera.transform.rotation = deltaRotation;

        if (currentRotation.x > 60 && currentRotation.x < 200)
        {
            currentRotation.x = Clamp(currentRotation.x, 1, 60);
        }
        else if (currentRotation.x < 270 && currentRotation.x > 200 )
        {
            currentRotation.x = Clamp(currentRotation.x, 270, 360);
        }

        camera.transform.rotation = Quaternion.Euler(currentRotation);

    }

    private void RotatePlayer(float turnY)
    {
        rotationY.Set(0f, turnY, 0f);
        rotationY = rotationY * sensitivityX;
        Quaternion deltaRotation = Quaternion.Euler(rotationY);
        rb.MoveRotation(rb.rotation * deltaRotation);
    }

    private void ResetCamera()
    {
//        rb.transform.rotation.x = GetComponentInChildren<Camera>.transform.rotation.x;
        Vector3 newRotation = camera.transform.rotation.eulerAngles;
        newRotation = new Vector3(0, newRotation.y, 0);

        camera.transform.rotation = Quaternion.Euler(camera.transform.rotation.x, camera.transform.rotation.y - newRotation.y , camera.transform.rotation.z);
        rb.transform.rotation = Quaternion.Euler(newRotation);
    }

    private float Clamp(float angle, float min, float max)
    {
        if (angle < min)
        {
            return min;
        }
        else if (angle > max)
            return max;
        else
            return angle;
    }

    // From the user input calculate using the set up speeds the velocity change
    private Vector3 CalculateVelocityChange(Vector3 inputVector)
    {
        // Calculate how fast we should be moving
        var relativeVelocity = transform.TransformDirection(inputVector);
        if (inputVector.z > 0)
        {
            relativeVelocity.z *= (canRun && Input.GetButton("Sprint")) ? runSpeed : walkSpeed;
        }
        else
        {
            relativeVelocity.z *= (canRun && Input.GetButton("Sprint")) ? runBackwardSpeed : walkBackwardSpeed;
        }
        relativeVelocity.x *= (canRunSidestep && Input.GetButton("Sprint")) ? runSidestepSpeed : sidestepSpeed;

        // Calcualte the delta velocity
        var currRelativeVelocity = rb.velocity - groundVelocity;
        var velocityChange = relativeVelocity - currRelativeVelocity;
        velocityChange.x = Mathf.Clamp(velocityChange.x, -maxVelocityChange, maxVelocityChange);
        velocityChange.z = Mathf.Clamp(velocityChange.z, -maxVelocityChange, maxVelocityChange);
        velocityChange.y = 0;

        return velocityChange;
    }

    // From the jump height and gravity we deduce the upwards speed for the character to reach at the apex.
    private float CalculateJumpVerticalSpeed()
    {
        return Mathf.Sqrt(2f * jumpHeight * Mathf.Abs(Physics.gravity.y));
    }

    // Check if the base of the capsule is colliding to track if it's grounded
    private void TrackGrounded(Collision collision)
    {
        var maxHeight = capsule.bounds.min.y + capsule.radius * .9f;
        foreach (var contact in collision.contacts)
        {
            if (contact.point.y < maxHeight)
            {
                if (isKinematic(collision))
                {
                    // Get the ground velocity and we parent to it
                    groundVelocity = collision.rigidbody.velocity;
                    transform.parent = collision.transform;
                }
                else if (isStatic(collision))
                {
                    // Just parent to it since it's static
                    transform.parent = collision.transform;
                }
                else
                {
                    // We are standing over a dinamic object,
                    // set the groundVelocity to Zero to avoid jiggers and extreme accelerations
                    groundVelocity = Vector3.zero;
                }

                // Esta en el suelo
                grounded = true;
            }

            break;
        }
    }

    private bool isKinematic(Collision collision)
    {
        return isKinematic(GetComponent<Collider>().transform);
    }

    private bool isKinematic(Transform transform)
    {
        return transform.GetComponent<Rigidbody>() && transform.GetComponent<Rigidbody>().isKinematic;
    }

    private bool isStatic(Collision collision)
    {
        return isStatic(collision.transform);
    }

    private bool isStatic(Transform transform)
    {
        return transform.gameObject.isStatic;
    }

    #endregion
}