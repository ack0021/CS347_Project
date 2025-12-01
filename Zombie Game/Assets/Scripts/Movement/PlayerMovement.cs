using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private CharacterController controller;
    public GunSystem1 gunSystem;

    [Header("Movement")]
    public float baseSpeed = 12f;
    public float speedMultiplier = 1f;
    public float gravity = -9.81f * 2;
    public float jumpHeight = 3f;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    [Header("Player Health")]
    public float maxHealth = 100f;
    public float currentHealth;
    public HealthBar healthBar;

    Vector3 velocity;
    bool isGrounded;

    public bool canMove = true;
    public GameOverUI gameOverUI;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        currentHealth = maxHealth;
    }

    void Update()
    {
        if (!canMove)
        {
            controller.Move(Vector3.zero);
            velocity = Vector3.zero;
            return;
        }

        // Ground check
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
            velocity.y = -2f;

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        // Camera relative movement
        Vector3 camForward = Camera.main.transform.forward;
        Vector3 camRight = Camera.main.transform.right;

        camForward.y = 0f;
        camRight.y = 0f;
        camForward.Normalize();
        camRight.Normalize();

        Vector3 move = camRight * x + camForward * z;

        // USE MULTIPLIED SPEED
        float finalSpeed = baseSpeed * speedMultiplier;
        controller.Move(move * finalSpeed * Time.deltaTime);

        if (Input.GetButtonDown("Jump") && isGrounded)
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;

        if (currentHealth <= 0f)
        {
            Die();
            gunSystem.Dead();

            if (gameOverUI != null)
                gameOverUI.ShowGameOver();
        }
    }

    private void Die()
    {
        canMove = false;
        Debug.Log("PLAYER DIED");

        PlayerCam camLook = Camera.main.GetComponent<PlayerCam>();
        if (camLook != null)
            camLook.enabled = false;

        if (gameOverUI != null)
            gameOverUI.ShowGameOver();
    }
}


