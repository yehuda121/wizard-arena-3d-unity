//using UnityEngine;

//// This script lets the player move and rotate while avoiding going through walls
//public class PlayerMovement : MonoBehaviour
//{
//    public float moveSpeed = 5f;          // Forward movement speed
//    public float rotationSpeed = 50f;    // Rotation speed in degrees per second
//    public float wallDetectionDistance = 0.2f; // Distance to detect walls in front

//    private Rigidbody rb;

//    void Start()
//    {
//        rb = GetComponent<Rigidbody>();
//        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
//    }

//    //void Update()
//    //{
//    //    float moveInput = 0f;
//    //    float rotateInput = 0f;

//    //    // Input collection
//    //    if (Input.GetKey(KeyCode.LeftArrow))
//    //        rotateInput = -1f;
//    //    else if (Input.GetKey(KeyCode.RightArrow))
//    //        rotateInput = 1f;

//    //    if (Input.GetKey(KeyCode.UpArrow))
//    //        moveInput = 1f;

//    //    // Apply rotation
//    //    if (rotateInput != 0f)
//    //    {
//    //        float rotationAmount = rotateInput * rotationSpeed * Time.deltaTime;
//    //        Quaternion turn = Quaternion.Euler(0f, rotationAmount, 0f);
//    //        rb.MoveRotation(rb.rotation * turn);
//    //    }

//    //    // Check for wall before moving
//    //    if (moveInput > 0f)
//    //    {
//    //        if (!Physics.Raycast(transform.position, transform.forward, wallDetectionDistance))
//    //        {
//    //            Vector3 moveDirection = transform.forward * moveSpeed * Time.deltaTime;
//    //            rb.MovePosition(rb.position + moveDirection);
//    //        }
//    //        else
//    //        {
//    //            //Debug.Log("Blocked by wall, cannot move forward.");
//    //        }
//    //    }
//    //}
//    void Update()
//    {
//        float moveInput = 0f;
//        float rotateInput = 0f;

//        // Read input for rotation
//        if (Input.GetKey(KeyCode.LeftArrow))
//            rotateInput = -1f;
//        else if (Input.GetKey(KeyCode.RightArrow))
//            rotateInput = 1f;

//        // Read input for forward movement
//        if (Input.GetKey(KeyCode.UpArrow))
//            moveInput = 1f;

//        // Apply rotation based on input
//        if (rotateInput != 0f)
//        {
//            float rotationAmount = rotateInput * rotationSpeed * Time.deltaTime;
//            Quaternion turn = Quaternion.Euler(0f, rotationAmount, 0f);
//            rb.MoveRotation(rb.rotation * turn);
//        }

//        Animator anim = GetComponentInChildren<Animator>();

//        // Handle forward movement
//        if (moveInput > 0f)
//        {
//            // Check if wall is blocking movement
//            if (!Physics.Raycast(transform.position, transform.forward, wallDetectionDistance))
//            {
//                Vector3 moveDirection = transform.forward * moveSpeed * Time.deltaTime;
//                rb.MovePosition(rb.position + moveDirection);

//                // Play walking animation if not blocked
//                if (anim != null)
//                    anim.SetBool("isWalking", true);
//            }
//            else
//            {
//                // Wall in the way, stop walking animation
//                if (anim != null)
//                    anim.SetBool("isWalking", false);
//            }
//        }
//        else
//        {
//            // No forward input, stop walking animation
//            if (anim != null)
//                anim.SetBool("isWalking", false);
//        }
//    }

//}
using UnityEngine;

// This script lets the player move and rotate while avoiding going through walls
public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;                  // Forward movement speed
    public float rotationSpeed = 50f;             // Rotation speed in degrees per second
    public float wallDetectionDistance = 0.2f;    // Distance to detect walls in front

    private Rigidbody rb;
    private SC_WizardAnimator animatorController;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;

        animatorController = GetComponent<SC_WizardAnimator>();
    }

    void Update()
    {
        float moveInput = 0f;
        float rotateInput = 0f;
        if (Input.GetKey(KeyCode.K))
        {
            Debug.Log("k press die");
            animatorController.PlayDeath();
        }

        // Read input for rotation
        if (Input.GetKey(KeyCode.LeftArrow))
            rotateInput = -1f;
        else if (Input.GetKey(KeyCode.RightArrow))
            rotateInput = 1f;

        // Read input for forward movement
        if (Input.GetKey(KeyCode.UpArrow))
            moveInput = 1f;

        // Apply rotation
        if (rotateInput != 0f)
        {
            float rotationAmount = rotateInput * rotationSpeed * Time.deltaTime;
            Quaternion turn = Quaternion.Euler(0f, rotationAmount, 0f);
            rb.MoveRotation(rb.rotation * turn);
        }

        // Handle movement and wall check
        bool isMoving = false;

        if (moveInput > 0f)
        {
            if (!Physics.Raycast(transform.position, transform.forward, wallDetectionDistance))
            {
                Vector3 moveDirection = transform.forward * moveSpeed * Time.deltaTime;
                rb.MovePosition(rb.position + moveDirection);
                isMoving = true;
            }
        }

        // Update walking animation based on actual movement
        animatorController?.SetWalking(isMoving);
    }
}

