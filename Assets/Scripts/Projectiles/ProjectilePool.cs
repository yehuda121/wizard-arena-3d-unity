using System.Collections.Generic;
using UnityEngine;

public class ProjectilePool : MonoBehaviour
{
    public GameObject projectilePrefab;
    public int poolSize = 5;

    private List<GameObject> projectiles;
    private int currentIndex = 0;

    void Start()
    {
        // create the pool
        projectiles = new List<GameObject>();
        for (int i = 0; i < poolSize; i++)
        {
            GameObject p = Instantiate(projectilePrefab, transform);
            p.SetActive(false); // not been used yet
            projectiles.Add(p);
        }
    }

    public GameObject GetNextProjectile()
    {
        GameObject p = projectiles[currentIndex];
        currentIndex = (currentIndex + 1) % poolSize;
        return p;
    }
}
