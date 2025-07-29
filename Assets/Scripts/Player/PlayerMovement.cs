using UnityEngine;

// This script lets the player move and rotate while avoiding going through walls
public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;                  // Forward movement speed
    public float rotationSpeed = 50f;             // Rotation speed in degrees per second
    public float wallDetectionDistance = 0.2f;    // Distance to detect walls in front

    private Rigidbody rb;
    private SC_WizardAnimator animatorController;
    private SC_PlayerHealthSystem playerHealth;

    void Start()
    {
        // Cache required components
        playerHealth = GetComponent<SC_PlayerHealthSystem>();
        rb = GetComponent<Rigidbody>();
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;

        animatorController = GetComponent<SC_WizardAnimator>();

        // Reset death state at game start
        PlayerState.isDead = false;
    }

    void Update()
    {
        bool alreadyMooving = PlayerState.isWalking;

        if (PlayerState.isDead || PlayerState.isShooting)
        {
            return;   
        }
        // Stop all movement if player is dead
        if (playerHealth != null && playerHealth.isDead)
        {
            PlayerState.isDead = true;
            PlayerState.isWalking = false;
            return;
        }

        float moveInput = 0f;
        float rotateInput = 0f;

        // Debug trigger: simulate death with key 'K'
        if (Input.GetKey(KeyCode.K))
        {
            animatorController.PlayDeath();
            //PlayerState.isDead = true;
        } 

        // Read input for rotation (left/right arrows)
        if (Input.GetKey(KeyCode.LeftArrow))
            rotateInput = -1f;
        else if (Input.GetKey(KeyCode.RightArrow))
            rotateInput = 1f;

        // Read input for forward movement (up arrow)
        if (Input.GetKey(KeyCode.UpArrow) && !PlayerState.isBlocking)
            moveInput = 1f;

        // Apply rotation
        if (rotateInput != 0f)
        {
            float rotationAmount = rotateInput * rotationSpeed * Time.deltaTime;
            Quaternion turn = Quaternion.Euler(0f, rotationAmount, 0f);
            rb.MoveRotation(rb.rotation * turn);
        }

        // Handle movement and wall collision check
        bool isMoving = false;

        if (moveInput > 0f)
        {
            // Check for wall ahead before moving
            if (!Physics.Raycast(transform.position, transform.forward, wallDetectionDistance))
            {
                Vector3 moveDirection = transform.forward * moveSpeed * Time.deltaTime;
                rb.MovePosition(rb.position + moveDirection);
                isMoving = true;
            }
        }

        // Update walking state globally and in animator
        PlayerState.isWalking = isMoving;
        if (!alreadyMooving)
            animatorController?.SetWalking(isMoving);
    }
}
