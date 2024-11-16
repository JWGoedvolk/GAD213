using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMove : MonoBehaviour
{
    // Lifetime of the projectile
    [SerializeField] public float lifeTime = 1f;
    [SerializeField] private float curTime = 0f;

    [Header("Physics")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Vector2 recordedVelocity;
    [SerializeField] private bool isPaused = false;

    [Header("Animations")]
    [SerializeField] private Animator animator;
    [SerializeField] private float animationTime = 0f;
    [SerializeField] private float explodeLength = 0.25f;

    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = gameObject.GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!isPaused)
        {
            // Check lifetime
            curTime += Time.deltaTime;
            if (curTime > lifeTime) // Check if the bullet died
            {
                animator.SetBool("Exploding", true);
                animationTime += Time.deltaTime;
                if (animationTime > explodeLength)
                {
                    Destroy(gameObject); // Destory the projectile if it is at the end of its life
                }
            }
        }
    }

    public void PauseBullet()
    {
        recordedVelocity = rb.velocity;
        rb.velocity = Vector2.zero;
        isPaused = true;
    }
    public void UnpauseBullet()
    {
        rb.AddForce(transform.up * recordedVelocity.magnitude, ForceMode2D.Impulse);
        isPaused = false;
    }
}
