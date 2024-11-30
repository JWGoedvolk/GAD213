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
        public List<string> TagWhitelist = new List<string>();
        [SerializeField] private bool destroyOnDamaged = true;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (TagWhitelist.Contains(collision.tag)) // We've triggered something that we should deal damage to
            {
                var health = collision.GetComponent<HealthManager>();
                if (health != null )
                {
                    health.Health = -Damage;
                    
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
                var health = collision.gameObject.GetComponent<HealthManager>();
                if (health != null)
                {
                    health.Health -= Damage;

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
    }
}