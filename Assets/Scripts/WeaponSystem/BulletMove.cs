using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace SAE.Weapons
{
    /// <summary>
    /// Moves the attached bullet projectile forward
    /// </summary>
    [RequireComponent(typeof(Rigidbody2D))]
    public class BulletMove : MonoBehaviour
    {
        [Header("Bullet Lifetime")]
        [SerializeField] public float lifeTime = 1f;
        [SerializeField] private float curTime = 0f;

        [Header("Physics")]
        [SerializeField] private Rigidbody2D rb;
        [SerializeField] private Vector2 recordedVelocity;

        [Header("Animations")]
        [SerializeField] private Animator animator;
        [SerializeField] private float explodeLength = 0.25f;

        void Awake()
        {
            // Automatically grab all necesary components
            rb = GetComponent<Rigidbody2D>();
            animator = gameObject.GetComponentInChildren<Animator>();

            // Get the length of the exploding animation
            AnimationClip[] clips = animator.runtimeAnimatorController.animationClips;
            foreach (var clip in clips)
            {
                if (clip.name == "FireBulletExplode")
                {
                    explodeLength = clip.length;
                }
                else
                {
                    continue;
                }
            }
        }

        void FixedUpdate()
        {
            if (!GameManager.IsPaused)
            {
                curTime += Time.deltaTime;
                if (curTime > lifeTime) // Check if the bullet died
                {
                    if (!animator.GetBool("Exploding"))
                    {
                        StartCoroutine(DestroyBullet());
                    }
                }
            }
        }

        public IEnumerator DestroyBullet()
        {
            Debug.Log("Destorying bullet");
            if (animator.GetBool("Exploding"))
            {
                yield return null; // We are already exploding so no need to explode again
            }

            // Set the animator to play the exploding animation then destroy the game object
            animator.SetBool("Exploding", true);
            yield return new WaitForSeconds(explodeLength);
            Destroy(gameObject);
        }

        /// <summary>
        /// Stops the bullet from moving when the game is paused
        /// </summary>
        public void PauseBullet()
        {
            recordedVelocity = rb.linearVelocity;
            rb.linearVelocity = Vector2.zero;
            animator.speed = 0f;
        }

        /// <summary>
        /// Starts the bullet moving like it did before being paused
        /// </summary>
        public void UnpauseBullet()
        {
            rb.AddForce(transform.up * recordedVelocity.magnitude, ForceMode2D.Impulse);
            animator.speed = 1f;
        }
    } 
}
