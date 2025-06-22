using UnityEngine;


public class SC_MagicProjectile : MonoBehaviour
{
    private void OnEnable()
    {
        // Reset and replay any ParticleSystem on this object and its children
        foreach (ParticleSystem ps in GetComponentsInChildren<ParticleSystem>())
        {
            ps.Clear();
            ps.Play();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Ignore collisions with unrelated objects
        if (!other.CompareTag("Enemy") && !other.CompareTag("BossEnemy"))
        {
            gameObject.SetActive(false);
            return;
        }

        float percentDamage = 0f;
        float maxHealth = 0f;

        if (other.CompareTag("BossEnemy"))
        {
            SC_BossEnemyHealthSystem bossHealth = other.GetComponent<SC_BossEnemyHealthSystem>();
            if (bossHealth != null)
            {
                percentDamage = 0.10f;
                maxHealth = bossHealth.maxHealth;
                bossHealth.TakeDamage(maxHealth * percentDamage);
                //Debug.Log("Hit Boss");
            }
            else
            {
                Debug.LogWarning("BossEnemyHealthSystem not found on Boss");
            }
        }
        else if (other.CompareTag("Enemy"))
        {
            SC_EnemyHealthSystem enemyHealth = other.GetComponent<SC_EnemyHealthSystem>();
            if (enemyHealth != null)
            {
                percentDamage = 0.25f;
                maxHealth = enemyHealth.maxHealth;
                enemyHealth.TakeDamage(maxHealth * percentDamage);
                //Debug.Log("Hit Enemy");
            }
            else
            {
                Debug.LogWarning("EnemyHealthSystem not found on Enemy");
            }
        }

        gameObject.SetActive(false);
    }

}
