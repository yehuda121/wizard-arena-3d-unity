using UnityEngine;

// This script handles player shooting behavior with automatic firing when holding spacebar
public class PlayerShooting : MonoBehaviour
{
    public Transform shootPoint;         // Where the projectile spawns
    public float shootForce = 15f;        // Speed of the projectile
    public float cooldown = 0.2f;         // Delay between shots (lower = faster shooting)

    private float lastShotTime = -999f;
    private PlayerProjectilePool projectilePool;

    void Start()
    {
        // Find the object pool manager in the scene
        projectilePool = FindObjectOfType<PlayerProjectilePool>();
    }

    void Update()
    {
        // Check if the Space key is being held and enough time passed since last shot
        if (Input.GetKey(KeyCode.Space) && Time.time > lastShotTime + cooldown)
        {
            Shoot();
            lastShotTime = Time.time;
        }
    }

    void Shoot()
    {
        // Get a projectile from the pool
        GameObject projectile = projectilePool.GetNextProjectile();

        // Position and rotate the projectile at the shoot point
        projectile.transform.position = shootPoint.position;
        projectile.transform.rotation = shootPoint.rotation;

        // Activate the projectile
        projectile.SetActive(true);

        // Apply velocity to the projectile in the forward direction
        Rigidbody rb = projectile.GetComponent<Rigidbody>();
        rb.velocity = shootPoint.forward * shootForce;
    }
}

