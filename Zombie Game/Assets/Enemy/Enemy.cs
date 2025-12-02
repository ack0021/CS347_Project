using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class Enemy : MonoBehaviour
{
    public float maxHealth = 100f;
    public float moveSpeed = 3.5f;

    public float attackRange = 2.5f;
    public float attackDamage = 10f;
    public float attackCooldown = 1.2f;
    private float nextAttackTime = 0f;

    public GameObject floatingDamageTextPrefab;
    public Canvas uiCanvas;

    [HideInInspector] public ZombieSpawner spawner;

    private float health;
    private bool isDead = false;

    private Rigidbody rb;
    private Transform target;
    private Animator animator;

    private Vector3 moveDirection;
    private float stoppingDistance = 2f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponentInChildren<Animator>();

        animator.applyRootMotion = false;
        rb.constraints = RigidbodyConstraints.FreezeRotation;
    }

    private void Start()
    {
        health = maxHealth;

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        target = playerObj.transform;

        StartCoroutine(GroundZombie());
    }

    private void Update()
    {
        if (isDead || target == null) return;

        float distance = Vector3.Distance(transform.position, target.position);

        Vector3 faceDir = target.position - transform.position;
        faceDir.y = 0f;
        transform.forward = faceDir.normalized;

        moveDirection = distance > stoppingDistance ?
            faceDir.normalized : Vector3.zero;

        GroundFollow();
        CheckAndPerformAttack();

        animator.SetFloat("Speed", moveDirection.magnitude);
    }

    private void FixedUpdate()
    {
        if (isDead) return;

        Vector3 flat = moveDirection;
        flat.y = 0;

        rb.MovePosition(rb.position + flat * moveSpeed * Time.fixedDeltaTime);
    }

    private void CheckAndPerformAttack()
    {
        if (Time.time < nextAttackTime) return;

        Collider[] hits = Physics.OverlapSphere(transform.position, attackRange);

        foreach (Collider hit in hits)
        {
            if (hit.CompareTag("Player"))
            {
                nextAttackTime = Time.time + attackCooldown;
                animator.SetTrigger("Attack");

                PlayerMovement p = hit.GetComponent<PlayerMovement>();
                if (p != null)
                    p.TakeDamage(attackDamage);

                return;
            }
        }
    }

    public void TakeDamage(float damageAmount)
    {
        if (isDead) return;

        health -= damageAmount;

        if (floatingDamageTextPrefab && uiCanvas)
        {
            Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position + Vector3.up * 1.8f);
            var go = Instantiate(floatingDamageTextPrefab, uiCanvas.transform);
            go.GetComponent<RectTransform>().position = screenPos;

            var fdt = go.GetComponent<FloatingDamageText>();
            if (fdt != null)
                fdt.SetText(Mathf.CeilToInt(damageAmount).ToString());
        }

        if (health <= 0) Die();
    }

    private void Die()
    {
        if (isDead) return;
        isDead = true;

        animator.SetTrigger("Die");

        spawner?.EnemyDied();

        var counter = FindObjectOfType<ZombiesKilledCounter>();
        counter?.IncrementKills();

        // Disable all colliders so bullets won't hit the dead zombie
        Collider[] colliders = GetComponentsInChildren<Collider>();
        foreach (Collider c in colliders)
        {
            c.enabled = false;
        }

        // Optional: disable rigidbody physics so corpse doesn't block
        Rigidbody[] rbs = GetComponentsInChildren<Rigidbody>();
        foreach (Rigidbody rb in rbs)
        {
            rb.isKinematic = true;
        }

        // Destroy after a short delay
        Destroy(gameObject, 1.5f);
    }


    private IEnumerator GroundZombie()
    {
        yield return new WaitForSeconds(0.05f);

        if (Physics.Raycast(transform.position + Vector3.up * 2f, Vector3.down, out RaycastHit hit, 10f))
        {
            Vector3 pos = transform.position;
            pos.y = hit.point.y;
            transform.position = pos;
        }
    }

    private void GroundFollow()
    {
        if (Physics.Raycast(transform.position + Vector3.up, Vector3.down, out RaycastHit hit, 5f))
        {
            Vector3 pos = transform.position;
            pos.y = Mathf.Lerp(pos.y, hit.point.y, Time.deltaTime * 15f);
            transform.position = pos;
        }
    }
}

