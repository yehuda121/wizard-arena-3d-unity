using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectilePool : MonoBehaviour
{
    public static PlayerProjectilePool Instance { get; private set; }

    [Header("Pool Settings")]
    public GameObject projectilePrefab;
    public int poolSize = 4;
    public int expandSize = 1;

    private Queue<GameObject> availableProjectiles;
    private List<GameObject> activeProjectiles;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Debug.LogWarning("[PlayerProjectilePool] Another instance detected, destroying...");
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
    }

    public GameObject GetNextProjectile()
    {
        if (availableProjectiles.Count == 0)
        {
            Debug.Log("[PlayerProjectilePool] Empty pool, expanding...");
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
            Debug.LogWarning("[PlayerProjectilePool] Tried to return projectile not in active list!");
        }
    }
}
