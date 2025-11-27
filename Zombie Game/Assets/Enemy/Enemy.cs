using System;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public static event Action<Enemy> OnEnemyKilled;

    [SerializeField] private float maxHealth = 100f;
    private float health;

    [SerializeField] private float moveSpeed = 5f;
    private Rigidbody rb;
    private Transform target;
    private Vector3 moveDirection;

    private Animator animator;

    [Header("Floating Damage Text")]
    public GameObject floatingDamageTextPrefab;

    private bool isDead = false;

    public ZombieSpawner spawner;
    public Canvas uiCanvas;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponentInChildren<Animator>();
    }

    private void Start()
    {
        health = maxHealth;
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
            target = playerObj.transform;

        if (spawner != null)
        {
            spawner = FindObjectOfType<ZombieSpawner>();
        }
    }

    private void Update()
    {
        if (isDead) return;

        if (target != null)
        {
            moveDirection = (target.position - transform.position).normalized;

            // Animator speed parameter
            float speed = moveDirection.magnitude * moveSpeed;
            if (animator != null)
                animator.SetFloat("Speed", speed);
        }
    }

    private void FixedUpdate()
    {
        if (isDead) return;

        if (moveDirection != Vector3.zero)
        {
            rb.MovePosition(transform.position + moveDirection * moveSpeed * Time.fixedDeltaTime);
            transform.forward = moveDirection;
        }
    }

    public void TakeDamage(float damageAmount)
    {
        if (isDead) return;

        health -= damageAmount;

        if (floatingDamageTextPrefab != null && uiCanvas != null)
        {
            // Convert enemy world position to screen position
            Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position + Vector3.up * 2f);

            // Instantiate as child of canvas
            GameObject dmgText = Instantiate(floatingDamageTextPrefab, uiCanvas.transform);

            // Set position in canvas space
            RectTransform rect = dmgText.GetComponent<RectTransform>();
            rect.position = screenPos;

            // Set the damage text
            FloatingDamageText fdt = dmgText.GetComponent<FloatingDamageText>();
            if (fdt != null)
                fdt.SetText(damageAmount.ToString());
        }

        if (health <= 0)
            Die();
    }

    private void Die()
    {
        isDead = true;

        if (animator != null)
            animator.SetTrigger("Die");

        OnEnemyKilled?.Invoke(this);

        // Destroy after death animation
        Destroy(gameObject, 1.5f);
    }

    void OnDestroy()
    {
        if (spawner != null)
            spawner.EnemyDied();
    }
}

