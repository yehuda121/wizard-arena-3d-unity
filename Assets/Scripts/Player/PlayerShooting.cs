// This script handles player shooting behavior.
// It spawns projectiles from a defined shoot point, using the projectile pool.
// The projectile now shoots forward based on the camera's direction, not the player's forward.

using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    public Transform shootPoint; // Where the projectile spawns
    public float shootForce = 15f; // Speed of the projectile
    public float cooldown = 1f; // Delay between shots

    private float lastShotTime = -999f;
    private ProjectilePool projectilePool;

    void Start()
    {
        // Find the object pool manager in the scene
        projectilePool = FindObjectOfType<ProjectilePool>();
    }

    void Update()
    {
        // Check if the player pressed the fire button and enough time passed since the last shot
        if (Input.GetButtonDown("Fire1") && Time.time > lastShotTime + cooldown)
        {
            Shoot();
            lastShotTime = Time.time;
        }
    }

    void Shoot()
    {
        // Get a projectile from the pool
        GameObject projectile = projectilePool.GetNextProjectile();

        // Position the projectile at the shoot point
        projectile.transform.position = shootPoint.position;

        projectile.transform.rotation = shootPoint.rotation;

        // Activate the projectile
        projectile.SetActive(true);

        // Get its Rigidbody and apply velocity in the direction the camera is facing
        Rigidbody rb = projectile.GetComponent<Rigidbody>();
        rb.velocity = shootPoint.forward * shootForce;
    }
}
