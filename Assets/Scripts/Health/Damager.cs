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
                    health.Health = -Damage;

                    if (collision.gameObject.CompareTag("Player") || (gameObject.tag == "PlayerBullet" && collision.tag == "Enemy"))
                    {
                        Debug.Log("Doing knockback on trigger");
                        Rigidbody2D playerRB = collision.gameObject.GetComponent<Rigidbody2D>();
                        playerRB.AddForce(transform.forward, ForceMode2D.Impulse);
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
                    health.Health = -Damage * elementalModifier;

                    if (collision.gameObject.CompareTag("Player") || (gameObject.tag == "PlayerBullet" && collision.gameObject.tag == "Enemy"))
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

        private void OnDrawGizmosSelected()
        {
            Gizmos.DrawRay(transform.position, direction * knockbackStrength);
        }
    }
}