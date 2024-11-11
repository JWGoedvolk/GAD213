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

    [Header("Animations")]
    [SerializeField] private Animator animator;
    [SerializeField] private float animationTime = 0f;
    [SerializeField] private float explodeLength = 0.25f;
    // *Damage stuff (probably not best to put it here)

    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = gameObject.GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Check lifetime
        curTime += Time.deltaTime;
        if (curTime > lifeTime)
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
