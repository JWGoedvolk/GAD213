using SAE.Weapons;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SAE.Health
{
    /// <summary>
    /// This script allows the attached game object to deal the set amount of damage to the collided or triggered object if it has a white listed tag
    /// </summary>
    [RequireComponent(typeof(CircleCollider2D))]
    public class Damager : MonoBehaviour
    {
        public float Damage = 1f;
        [SerializeField] private float damageCooldown = 0.1f;
        [SerializeField] private bool hasDealtDamage = false;
        public float knockbackStrength = 2f;
        [SerializeField] private Vector2 direction;
        public List<string> TagWhitelist = new List<string>();
        [SerializeField] private bool destroyOnDamaged = true;
        
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (TagWhitelist.Contains(collision.gameObject.tag)) // We've triggered something that we should deal damage to
            {
                var health = collision.gameObject.GetComponent<HealthManager>();
                if (health != null )
                {
                    if (!hasDealtDamage)
                    {
                        health.Health = -Damage;
                        hasDealtDamage= true;
                        StartCoroutine(DamageCooldown());
                        if (collision.gameObject.tag == "Enemy")
                        {
                            Rigidbody2D enemyRB = collision.gameObject.GetComponent<Rigidbody2D>();
                            direction = (enemyRB.transform.position - transform.position).normalized;
                            enemyRB.AddForce(direction * knockbackStrength, ForceMode2D.Impulse);
                        }
                    }
                    
                    BulletMove bullet = GetComponent<BulletMove>();
                    if (bullet != null)
                    {
                        StartCoroutine(bullet.DestroyBullet());
                    }
                    else 
                    {
                        if (destroyOnDamaged)
                        {
                            Destroy(gameObject);
                        }
                    }
                }
            }
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (TagWhitelist.Contains(collision.gameObject.tag)) // We've collided with something that we should deal damage to
            {
                Debug.Log($"{gameObject.name} collided with {collision.gameObject.tag}");
                var health = collision.gameObject.GetComponent<HealthManager>();
                if (health != null)
                {
                    
                    ElementalManager attackingManager = GetComponent<ElementalManager>();
                    ElementalManager defendingManager = collision.gameObject.GetComponent<ElementalManager>();
                    float elementalModifier = defendingManager.CalculateDamageModifier(attackingManager.element);
                    if (!hasDealtDamage)
                    {
                        health.Health = -Damage * elementalModifier;
                        hasDealtDamage = true;
                        StartCoroutine(DamageCooldown());
                    }

                    if (collision.gameObject.CompareTag("Player"))
                    {
                        Debug.Log("Doing knockback on collosion");
                        Rigidbody2D playerRB = collision.gameObject.GetComponent<Rigidbody2D>();
                        direction = (playerRB.transform.position - transform.position).normalized;
                        playerRB.AddForce(direction * knockbackStrength, ForceMode2D.Impulse);
                        return; // We collided with a player, so no need to do bullet stuff further
                    }

                    BulletMove bullet = collision.gameObject.GetComponent<BulletMove>();
                    if (bullet != null)
                    {
                        bullet.DestroyBullet();
                    }
                    else 
                    {
                        if (destroyOnDamaged)
                        {
                            Destroy(gameObject);
                        }
                    }
                }
            }
        }

        private IEnumerator DamageCooldown()
        {
            yield return new WaitForSeconds(damageCooldown);
            hasDealtDamage = false;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.DrawRay(transform.position, direction * knockbackStrength);
        }
    }
}