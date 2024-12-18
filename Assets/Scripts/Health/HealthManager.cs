using JW.GPG.Analytics;
using SAE.EventSystem;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

namespace SAE.Health
{
    /// <summary>
    /// This script stores health and allows for it to be damaged and/or healed, trigger events for the respective events happening.
    /// </summary>
    public class HealthManager : MonoBehaviour
    {
        // TODO: Add custom inspector for showing only player/enemy relevant variables depending on target tag
        
        [SerializeField] private float health;
        [SerializeField] private float invulTime = 0.5f;
        [SerializeField] private bool isInvul = false;
        [SerializeField] Animator animator;
        [SerializeField] float explodeLength;
        [SerializeField] float damageLength;
        [SerializeField] bool isDamaged = false;
        [SerializeField] GameObject xpOrb;
        [SerializeField] GameEventScriptable enemyKilled;
        public float Health
        {
            get { return health; }
            set
            {
                //Debug.LogWarning(value);
                if (!isInvul || value > 0f)
                {
                    health += value;
                }

                if (value <= 0f)
                {
                    Debug.Log("Invoking OnDamaged");
                    if (OnDamaged != null) 
                    { 
                        OnDamaged?.Invoke(); 
                    }

                    // Play the damaged animation if we are on an enemy object
                    if (gameObject.CompareTag("Enemy"))
                    {
                        Debug.Log("Damage animation clip should play");
                        if (!animator.GetBool("IsDamaged")) // Only play the animation if it isn't already
                        {
                            StartCoroutine(Damaged(damageLength)); // Plays the animation for the length of the damage clip
                        }
                    }
                    else if (gameObject.CompareTag("Player") && !isInvul)
                    {
                        Debug.Log("Damaging player");
                        // Invul
                        if (!isInvul)
                        {
                            StartCoroutine(InvulTimer(invulTime));
                        }
                    }
                }
                else if (value > 0f) 
                { 
                    if (OnHeal != null) OnHeal?.Invoke(); 
                }

                if (health <= 0f)
                {
                    if (gameObject.CompareTag("Enemy"))
                    {
                        if (!animator.GetBool("IsExploding"))
                        {
                            // Explode the enemy
                            animator.SetBool("IsExploding", true);
                            StartCoroutine(Explode(explodeLength));
                            Instantiate(xpOrb, transform.position, Quaternion.identity);
                        }
                        enemyKilled.Raise();
                    }
                    else if (gameObject.CompareTag("Player"))
                    {
                        AnalyticGameEvent.OnPlayrDeath(AnalyticManager.Instance); // Log analytic data
                        gameObject.SetActive(false);
                        Debug.Log("Player killed");
                    }
                    if (OnDeath != null) 
                    {
                        OnDeath?.Invoke();   
                    }
                }
            }
        }
        [SerializeField] private UnityEvent OnDeath;
        [SerializeField] private UnityEvent OnDamaged;
        [SerializeField] private UnityEvent OnHeal;

        private void Awake()
        {
            // Get the length of the exploding animation
            explodeLength = 0f;
            AnimationClip[] clips = animator.runtimeAnimatorController.animationClips;
            foreach (var clip in clips)
            {
                switch (clip.name)
                {
                    case "EnemyExplode":
                        explodeLength = clip.length; 
                        break;
                    case "Damaged":
                        damageLength = clip.length;
                        break;
                    default:
                        break;
                }
            }
        }

        public async void PlayDamaged()
        {
            animator.SetBool("IsDamaged", true);
            StartCoroutine(Damaged(damageLength));
            while (isDamaged)
            {
                await Task.Delay(10);
            }
            animator.SetBool("IsDamaged", false);
        }

        private IEnumerator Damaged(float clipLength)
        {
            isDamaged = true;
            yield return new WaitForSeconds(clipLength);
            isDamaged = false;
        }

        private IEnumerator Explode(float clipLength)
        {
            Debug.Log("Exploding enemy from coroutine");

            yield return new WaitForSeconds(clipLength);
            Destroy(gameObject);
        }

        private IEnumerator InvulTimer(float time)
        {
            isInvul = true;
            yield return new WaitForSeconds(time);
            isInvul = false;
        }
    } 
}
