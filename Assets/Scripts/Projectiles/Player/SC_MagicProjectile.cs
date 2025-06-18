using UnityEngine;

// Handles projectiles shot by the player
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
        // Collision with anything else (not valid targets)
        if (!other.CompareTag("Enemy") && !other.CompareTag("BossEnemy"))
        {
            gameObject.SetActive(false);
            return;
        }

        // Player shot and hit an enemy
        SC_EnemyHealthSystem enemyHealth = other.GetComponent<SC_EnemyHealthSystem>();
        if (enemyHealth != null)
        {
            float percentDamage = 0f;

            if (other.CompareTag("BossEnemy"))
            {
                percentDamage = 0.10f; // 10% damage to boss
                //Debug.Log("i hit the boss");
            }
            else if (other.CompareTag("Enemy"))
            {
                percentDamage = 0.25f; // 25% damage to regular enemy
            }

            if (percentDamage > 0f)
            {
                float damage = enemyHealth.maxHealth * percentDamage;
                enemyHealth.TakeDamage(damage);
                gameObject.SetActive(false);
                return;
            }


            // Just in case damage percent is 0 but still hit something
            gameObject.SetActive(false);
        }
    }
}

