using UnityEngine;

public class SC_MagicProjectile : MonoBehaviour
{
    private void OnEnable()
    {
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
                bossHealth.TakeDamage(bossHealth.maxHealth * 0.10f);
            }
        }
        else if (other.CompareTag("Enemy"))
        {
            SC_EnemyHealthSystem enemyHealth = other.GetComponent<SC_EnemyHealthSystem>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(enemyHealth.maxHealth * 0.25f);
            }
        }

        GetComponent<PlayerProjectileAutoDisable>()?.ReturnToPool();
    }
}
