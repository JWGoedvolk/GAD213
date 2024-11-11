using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSystem : MonoBehaviour
{
    [Header("Bullets")]
    [SerializeField] private BulletScriptable bulletStat;
    [SerializeField] private GameObject bulletPrefab;

    [Header("Weapons")]
    [SerializeField] private Transform firePoint;
    [SerializeField] private WeaponScriptables weaponStats;
    [SerializeField] private float fireTime = 0f;
    [SerializeField] private bool isFireable = true;
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
        if (!isFireable)
        {
            fireTime += Time.deltaTime;
            if (fireTime >= weaponStats.FireRate)
            {
                isFireable = true;
                fireTime = 0f;
            }
        }

        if (Input.GetKeyDown(fireKey) && isFireable)
        {
            // Spawn the bullet
            var bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);

            // Shoot the bullet forward
            speed = weaponStats.Velocity * bulletStat.VelocityModifier;
            bullet.GetComponent<Rigidbody2D>().AddForce(firePoint.up * (player.velocity.magnitude + speed), ForceMode2D.Impulse);

            isFireable = false;
        }
    }
}
