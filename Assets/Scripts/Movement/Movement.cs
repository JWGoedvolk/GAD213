using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Author: Jan-Willem Goedvolk
/// Purpose: Handle player movement
/// </summary>
public class Movement : MonoBehaviour
{
    [Header("Player Move Speed")]
    [SerializeField] private Rigidbody2D body;
    [SerializeField] private bool isThrusting = false;
    [SerializeField] private FloatVarScriptable speed; // This is the speed we are trying to reach when thrusting
    [SerializeField] private FloatVarScriptable maxSpeed;
    [SerializeField] private float curSpeed = 0f;
    [SerializeField] private float speedTimeStepFactor = 1f;

    [Header("Player Turning Speed")]
    [SerializeField] private FloatVarScriptable turnSpeed; // This is what we are trying to reach
    [SerializeField] private float curTurnSpeed = 0f;
    [SerializeField] private float maxTurnSpeed = 1f;
    [SerializeField] private float turnSpeedStepFactor = 1f;
    [SerializeField] private float turnDragFactor = 1f;

    [Header("Animations")]
    [SerializeField] private Animator animator;

    [Header("Pausing")]
    [SerializeField] private Vector2 recordedVelocity;
    [SerializeField] private float recordedAngularVelocity;
    [SerializeField] private bool isPaused = false;

    // Update is called once per frame
    void Update()
    {
        if (!isPaused)
        {
            if (Input.GetKey(KeyCode.Space))
            {
                isThrusting = true;
                animator.SetBool("IsMoving", isThrusting);

                curSpeed = Mathf.Lerp(curSpeed, speed.Value, Time.deltaTime * speedTimeStepFactor);
            }
            else
            {
                isThrusting = false;
                animator.SetBool("IsMoving", isThrusting);

                curSpeed = Mathf.MoveTowards(curSpeed, 0f, Time.deltaTime * 2 * speedTimeStepFactor);
            }

            if (Input.GetKey(KeyCode.A))
            {
                curTurnSpeed = Mathf.MoveTowards(curTurnSpeed, turnSpeed.Value, Time.deltaTime * turnSpeedStepFactor);
            }
            else if (Input.GetKey(KeyCode.D))
            {
                curTurnSpeed = Mathf.MoveTowards(curTurnSpeed, -turnSpeed.Value, Time.deltaTime * turnSpeedStepFactor);
            }
            else
            {
                curTurnSpeed = Mathf.MoveTowards(curTurnSpeed, 0f, Time.deltaTime * turnDragFactor);
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    private void FixedUpdate()
    {
        if (!isPaused)
        {
            if (isThrusting) // Move us forward
            {
                body.AddForce(transform.up * curSpeed);
            }

            transform.Rotate(Vector3.forward, curTurnSpeed);

            body.velocity = Vector2.ClampMagnitude(body.velocity, maxSpeed.Value); // Limit our speed
        }
    }

    public void PauseMovement()
    {
        isPaused = true;

        // Record the speeds and rotations
        recordedVelocity = body.velocity;
        recordedAngularVelocity = body.angularVelocity;

        // Set the speeds and rotations to stop
        body.velocity = Vector2.zero;
        body.angularVelocity = 0f;
    }

    public void UnpauseMovement()
    {
        isPaused = false;

        body.velocity = recordedVelocity;
        body.angularVelocity = recordedAngularVelocity;
    }
}
