// This script handles player shooting behavior.
// It spawns projectiles from a defined shoot point, using the projectile pool.

using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    public Transform shootPoint;
    public float shootForce = 15f;
    public float cooldown = 1f;

    private float lastShotTime = -999f;
    private ProjectilePool projectilePool;

    void Start()
    {
        projectilePool = FindObjectOfType<ProjectilePool>();
    }

    void Update()
    {
        if (Input.GetButtonDown("Fire1") && Time.time > lastShotTime + cooldown)
        {
            Shoot();
            lastShotTime = Time.time;
        }
    }

    void Shoot()
    {
        GameObject projectile = projectilePool.GetNextProjectile();
        projectile.transform.position = shootPoint.position;
        projectile.transform.rotation = shootPoint.rotation;
        projectile.SetActive(true);

        Rigidbody rb = projectile.GetComponent<Rigidbody>();
        rb.velocity = shootPoint.forward * shootForce;
    }
}
