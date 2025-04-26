using UnityEngine;

// This script makes the player rotate left/right and move forward in the direction he's facing.
// The down arrow will be used later for aim mode.

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;        // Forward movement speed
    public float rotationStep = 5f;    // Degrees per frame
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        float rotateInput = 0f;
        // Left/Right arrows (or A/D)
        if (Input.GetKey(KeyCode.LeftArrow))
            rotateInput = -1f;
        else if (Input.GetKey(KeyCode.RightArrow))
            rotateInput = 1f;
        
        float moveInput = Input.GetKey(KeyCode.UpArrow) ? 1f : 0f;

        // Rotate in place (or while moving) with fixed speed
        if (rotateInput != 0f)
        {
            float rotationAmount = rotateInput * rotationStep;
            transform.Rotate(0f, rotationAmount, 0f);
        }
        else
        {
            // Clear physics-based rotation only if Rigidbody is not kinematic
            if (rb != null && !rb.isKinematic)
            {
                rb.angularVelocity = Vector3.zero;
            }
        }

        // Move forward only if UpArrow is held
        if (moveInput > 0f)
        {
            Vector3 moveDir = transform.forward * moveSpeed * Time.deltaTime;
            rb.MovePosition(rb.position + moveDir);
        }
    }
}
