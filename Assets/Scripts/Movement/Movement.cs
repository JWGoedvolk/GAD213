using SAE.Variavles;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SAE.Movement.Player
{
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
            if (!GameManager.IsPaused)
            {
                // Thrusting
                if (Input.GetAxis("Fire2") > 0f)
                {
                    isThrusting = true;
                    animator.SetBool("IsMoving", isThrusting);

                    curSpeed = Mathf.Lerp(curSpeed, speed.Value, Time.deltaTime * speedTimeStepFactor);
                }
                else // Not thrusting, so drifting physics
                {
                    isThrusting = false;
                    animator.SetBool("IsMoving", isThrusting);

                    curSpeed = Mathf.MoveTowards(curSpeed, 0f, Time.deltaTime * 2 * speedTimeStepFactor);
                }

                var horizontal = Input.GetAxis("Horizontal");
                if (horizontal < 0f || horizontal > 0f) // Turning
                {
                    curTurnSpeed = Mathf.MoveTowards(curTurnSpeed, turnSpeed.Value * -horizontal, Time.deltaTime * turnSpeedStepFactor);
                }
                else // Not turning
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
            if (!GameManager.IsPaused)
            {
                if (isThrusting) // Move us forward
                {
                    body.AddForce(transform.up * curSpeed);
                }

                transform.Rotate(Vector3.forward, curTurnSpeed); // Alway be applying rotations so we can drift when not thrusting

                body.velocity = Vector2.ClampMagnitude(body.velocity, maxSpeed.Value); // Limit our speed
            }
        }

        /// <summary>
        /// This esseantially stops the attached game object from moving
        /// </summary>
        public void PauseMovement()
        {
            Debug.Log("Pausing player movement");

            isPaused = true;
            animator.speed = 0f;

            // Record the speeds and rotations
            recordedVelocity = body.velocity;
            recordedAngularVelocity = body.angularVelocity;

            // Set the speeds and rotations to stop
            body.velocity = Vector2.zero;
            body.angularVelocity = 0f;
            body.Sleep();
        }

        /// <summary>
        /// This allows the attached game object to continue moving with the same speed and rotation as before
        /// </summary>
        public void UnpauseMovement()
        {
            Debug.Log("Unpausing player movement");
            
            isPaused = false;
            animator.speed = 1f;

            body.WakeUp();
            body.velocity = recordedVelocity;
            body.angularVelocity = recordedAngularVelocity;
        }
    } 
}
