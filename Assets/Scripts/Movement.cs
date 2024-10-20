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
    [SerializeField] private FloatVarScriptable speed;
    [SerializeField] private FloatVarScriptable maxSpeed;
    [SerializeField] private float curSpeed = 0f;
    [SerializeField] private float speedTimeStepFactor = 1f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.Space))
        {
            isThrusting = true;
            curSpeed = Mathf.Lerp(curSpeed, speed.Value, Time.deltaTime * speedTimeStepFactor);
        }
    }

    private void FixedUpdate()
    {
        if (isThrusting) // Move us forward
        {
            body.velocity = transform.up * curSpeed;
        }

        body.velocity = Vector2.ClampMagnitude(body.velocity, maxSpeed.Value); // Limit our speed
    }
}
