using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectilePool : MonoBehaviour
{
    public GameObject projectilePrefab;
    public int poolSize = 10;

    private List<GameObject> projectiles;
    private int currentIndex = 0;

    void Start()
    {
        projectiles = new List<GameObject>();

        for (int i = 0; i < poolSize; i++)
        {
            GameObject p = Instantiate(projectilePrefab, transform);
            p.SetActive(false);
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
