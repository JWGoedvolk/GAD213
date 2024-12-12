using SAE.Health;
using System.Collections;
using UnityEngine;

public class BlackHole : MonoBehaviour
{
    [Header("Blackhole Movement")]
    [SerializeField] private Rigidbody2D rb;
    public Transform Target;
    [SerializeField] float speed;
    [SerializeField] float turnSpeed = 1.0f;

    [Header("Blackhole Pulling")]
    public GameObject Player;
    [SerializeField] private Rigidbody2D playerRB;
    [SerializeField] private float range = 5f;
    [SerializeField] private float intensity = 1f;
    [SerializeField] private Vector2 directionToPlayer = Vector2.zero;
    [SerializeField] private Vector2 pullForce = Vector2.zero;

    [Header("Damaging")]
    [SerializeField] private HealthManager playerHealth;
    [SerializeField] private float damageRate = 0.1f;
    [SerializeField] private float damageInterval = 0.5f;
    [SerializeField] private bool PlayerInDamageRane = false;

    public void StartHazard()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.linearVelocity = (Target.position - transform.position).normalized * speed;

        if (playerRB == null)
        {
            playerRB = Player.GetComponent<Rigidbody2D>();
        }

        rb.AddTorque(turnSpeed);

        //StartCoroutine(DamagePlayer());
    }

    // Update is called once per frame
    void Update()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, Player.transform.position);
        directionToPlayer = transform.position - Player.transform.position;
        if (distanceToPlayer <= range)
        {
            pullForce = directionToPlayer.normalized / distanceToPlayer * intensity;
            playerRB.AddForce(pullForce, ForceMode2D.Force);
        }
        
        //if (PlayerInDamageRane)
        //{
        //    playerHealth.Health -= damageRate;
        //}
    }

    IEnumerator DamagePlayer()
    {
        while (PlayerInDamageRane)
        {
            if (playerHealth != null)
            {
                playerHealth.Health = -damageRate;
                yield return new WaitForSeconds(damageInterval);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerHealth = collision.GetComponent<HealthManager>();
            if (playerHealth != null)
            {
                PlayerInDamageRane = true;
                StartCoroutine(DamagePlayer());
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerHealth = null;
            PlayerInDamageRane = false;
            StopCoroutine(DamagePlayer());
        }
    }

    private void OnDrawGizmosSelected()
    {

        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, range);
        if (Target != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, Target.position);
        }
    }
}
