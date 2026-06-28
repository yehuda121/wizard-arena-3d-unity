using UnityEngine;

public class SC_MagicProjectile : MonoBehaviour
{
    private const float EnemyDamageFraction = 0.25f;
    private const float BossDamageFraction = 0.10f;

    [HideInInspector]
    public float damageMultiplier = 1f;

    private void OnEnable()
    {
        damageMultiplier = 1f;

        foreach (ParticleSystem ps in GetComponentsInChildren<ParticleSystem>())
        {
            ps.Clear();
            ps.Play();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Enemy") && !other.CompareTag("BossEnemy"))
        {
            GetComponent<PlayerProjectileAutoDisable>()?.ReturnToPool();
            return;
        }

        if (other.CompareTag("BossEnemy"))
        {
            SC_BossEnemyHealthSystem bossHealth = other.GetComponent<SC_BossEnemyHealthSystem>();
            if (bossHealth != null)
            {
                bossHealth.TakeDamage(bossHealth.maxHealth * BossDamageFraction * damageMultiplier);
            }
        }
        else if (other.CompareTag("Enemy"))
        {
            SC_EnemyHealthSystem enemyHealth = other.GetComponent<SC_EnemyHealthSystem>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(enemyHealth.maxHealth * EnemyDamageFraction * damageMultiplier);
            }
        }

        GetComponent<PlayerProjectileAutoDisable>()?.ReturnToPool();
    }
}
