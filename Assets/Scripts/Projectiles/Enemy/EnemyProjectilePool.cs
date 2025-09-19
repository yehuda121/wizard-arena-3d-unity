using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectilePool : MonoBehaviour
{
    public static EnemyProjectilePool Instance { get; private set; } // Singleton

    [Header("Pool Settings")]
    public GameObject projectilePrefab;
    public int poolSize = 3;
    public int expandSize = 1;

    private Queue<GameObject> availableProjectiles;  // free projectiles
    private List<GameObject> activeProjectiles;      // in-use projectiles

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Debug.LogWarning("[EnemyProjectilePool] Another instance detected, destroying...");
            Destroy(gameObject);
            return;
        }
        Instance = this;

        availableProjectiles = new Queue<GameObject>();
        activeProjectiles = new List<GameObject>();

        AddProjectiles(poolSize);
    }

    private void AddProjectiles(int count)
    {
        for (int i = 0; i < count; i++)
        {
            GameObject p = Instantiate(projectilePrefab, transform);
            p.SetActive(false);
            availableProjectiles.Enqueue(p);
        }

        Debug.Log("[EnemyProjectilePool] Expanded by " + count + " projectiles. Total = " +
                  (availableProjectiles.Count + activeProjectiles.Count));
    }

    public GameObject GetNextProjectile()
    {
        if (availableProjectiles.Count == 0)
        {
            Debug.Log("[EnemyProjectilePool] Empty pool, expanding...");
            AddProjectiles(expandSize);
        }

        GameObject proj = availableProjectiles.Dequeue();
        proj.SetActive(true);
        activeProjectiles.Add(proj);
        return proj;
    }

    public void ReturnProjectile(GameObject proj)
    {
        if (activeProjectiles.Remove(proj))
        {
            proj.SetActive(false);
            availableProjectiles.Enqueue(proj);
        }
        else
        {
            Debug.LogWarning("[EnemyProjectilePool] Tried to return projectile not in active list!");
        }
    }
}
