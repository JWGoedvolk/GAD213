using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SAE.Upgrades
{
    [RequireComponent(typeof(Collider2D))]
    public class ExperienceCollector : MonoBehaviour
    {
        [SerializeField] private float lifeTime = 5f;
        [SerializeField] private float amount = 1f;
        public bool isPaused = false;
        private Animator animator;

        private void Awake()
        {
            isPaused = false;
            animator = GetComponent<Animator>();
        }

        private void Update()
        {
            if (!isPaused)
            {
                lifeTime -= Time.deltaTime;
                if (lifeTime < 0)
                {
                    Destroy(gameObject);
                }
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                ExperienceHandler experienceHandler = collision.GetComponent<ExperienceHandler>();
                experienceHandler.CollectXP(amount);

                Destroy(gameObject); // TODO: Implement ObjectPoolers
            }
        }

        public void SetPause(bool pause)
        {
            isPaused = pause;
            animator.speed = isPaused ? 0f : 1f;
        }
    } 
}
