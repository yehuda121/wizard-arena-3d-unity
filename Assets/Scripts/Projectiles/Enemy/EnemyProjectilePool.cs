using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectilePool : MonoBehaviour
{
    public GameObject projectilePrefab;
    public int poolSize = 30;

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

        if (!p.activeSelf)
        {
            p.SetActive(true);
            //Debug.Log("Activating projectile from pool at index: " + currentIndex);
        }
        else
        {
            Debug.LogWarning("Projectile already active at index: " + currentIndex + ". Forcing reset.");
            p.SetActive(false); // קודם לכבות
            p.SetActive(true);  // ואז להפעיל מחדש
        }

        currentIndex = (currentIndex + 1) % poolSize;
        return p;
    }


}
