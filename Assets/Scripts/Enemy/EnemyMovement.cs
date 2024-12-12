using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SAE.Movement.Player;
using SAE.Weapons;
using UnityEngine.UI;

namespace SAE.Movement.Enemy
{
    /// <summary>
    /// This class is responsible for moving the enemy towards the player at all times
    /// </summary>
    public class EnemyMovement : MonoBehaviour
    {
        [SerializeField] private GameObject player;

        [SerializeField] private float moveSpeed = 1f;
        [SerializeField] private float rotateSpeed = 2f;
        [SerializeField] private Rigidbody2D rb;
        public bool isPaused = false;
        private Vector2 recordedVelocity;
        private Animator anim;

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            player = GameObject.FindWithTag("PlayerTrack");
            anim = GetComponentInChildren<Animator>();
        }

        void Update()
        {
            if (!isPaused)
            {
                // Turn towards the player
                Vector3 vectorToTarget = player.transform.position - transform.position;
                float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg;
                Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
                transform.rotation = Quaternion.Slerp(transform.rotation, q, Time.deltaTime * rotateSpeed);

                // Move forward towards the player
                rb.linearVelocity = transform.right * moveSpeed;
            }
        }

        public void SetPause(bool pause)
        {
            isPaused = pause;
            if (pause)
            {
                recordedVelocity = rb.linearVelocity;
                rb.linearVelocity = Vector2.zero;
                anim.speed = 0f;
            }
            else
            {
                rb.linearVelocity = recordedVelocity;
                anim.speed = 1f;
            }
        }
    } 
}
