using SAE.Health;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SAE.Weapons.Weapons;
using SAE.Weapons.Bullets;

namespace SAE.Weapons
{
    /// <summary>
    /// This script handles the player firing their weapon
    /// </summary>
    public class WeaponSystem : MonoBehaviour
    {
        [Header("Bullets")]
        [SerializeField] public BulletScriptable bulletStat;
        [SerializeField] private GameObject bulletPrefab;
        [SerializeField] public float bulletSpeedModifier;
        [SerializeField] public float bulletSizeModifier;

        [Header("Weapons")]
        [SerializeField] private bool isPaused = false;
        [SerializeField] private Transform firePoint;
        [SerializeField] public WeaponScriptables weaponStats;
        [SerializeField] private float fireTime = 0f;
        [SerializeField] public float fireRateModifier = 1f;
        [SerializeField] public bool isFireable = true;
        [SerializeField] private KeyCode fireKey = KeyCode.K;

        [Header("Stored Stats")]
        [SerializeField] private float speed;
        [SerializeField] private Rigidbody2D player;


        // Start is called before the first frame update
        void Start()
        {
            player = GetComponent<Rigidbody2D>();
        }

        // Update is called once per frame
        void Update()
        {
            if (!isPaused)
            {
                if (!isFireable)
                {
                    fireTime += Time.deltaTime;
                    if (fireTime >= weaponStats.FireRate * fireRateModifier)
                    {
                        isFireable = true;
                        fireTime = 0f;
                    }
                }

                if (weaponStats.doesAutofire) // Continuos firing
                {
                    if (Input.GetAxis("Fire1") > 0f && isFireable)
                    {
                        ShootBullet();
                    }
                }
                else // Needs to lift and press key again
                {
                    if (Input.GetAxis("Fire1") > 0f && isFireable)
                    {
                        ShootBullet();
                    }
                }

            }
        }

        /// <summary>
        /// Instantiates the bullet and sets its inheritted speed from the player as well as any other stats that need to be set to get the bullet working
        /// </summary>
        private void ShootBullet()
        {
            // Spawn the bullet
            var bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            bullet.transform.localScale = Vector3.one * bulletSizeModifier;
            // Shoot the bullet forward
            speed = (weaponStats.Velocity * bulletStat.VelocityModifier) * bulletSpeedModifier;
            bullet.GetComponent<Rigidbody2D>().AddForce(firePoint.up * (player.velocity.magnitude + speed), ForceMode2D.Impulse);

            // Sets bullet damage
            bullet.GetComponent<Damager>().Damage = bulletStat.Damage * weaponStats.DamageModifier;

            // Stops us from firing before the cooldown is over
            isFireable = false;
        }

        public void PauseSystem()
        {
            isPaused = true;
        }
        public void UnPauseSystem()
        {
            isPaused = false;
        }
    } 
}
