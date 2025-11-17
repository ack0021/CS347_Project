using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Enemy : MonoBehaviour
{
    public static event Action<Enemy> OnEnemyKilled;
    [SerializeField] float health, maxHealth = 3f;

    [SerializeField] float moveSpeed = 5f;
    Rigidbody rb;
    Transform target;
    Vector3 moveDirection;

    Animator animator;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();

        // Works even if animator is on a child bone/model
        animator = GetComponentInChildren<Animator>();

        if (rb == null) Debug.LogError("Rigidbody missing on skeleton!");
        if (animator == null) Debug.LogError("Animator missing on skeleton!");
    }

    // Start is called before the first frame update
    private void Start()
    {
        health = maxHealth;
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        if (target)
        {
            moveDirection = (target.position - transform.position).normalized;

            float speed = moveDirection.magnitude * moveSpeed;
            animator.SetFloat("Speed", speed);
        }
    }

    private void FixedUpdate()
    {
        if (moveDirection != Vector3.zero)
        {
            rb.MovePosition(transform.position + moveDirection * moveSpeed * UnityEngine.Time.deltaTime);
            transform.forward = moveDirection;
        }
    }

    public void takeDamage(float damageAmount)
    {
        health -= damageAmount;

        if (health <= 0)
        {
            animator.SetTrigger("Die");
            OnEnemyKilled?.Invoke(this);
            Destroy(gameObject, 1.5f);
            
        }
    }

}
