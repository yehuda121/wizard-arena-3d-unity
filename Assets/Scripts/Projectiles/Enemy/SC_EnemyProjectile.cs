using UnityEngine;

public class SC_EnemyProjectile : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SC_PlayerHealthSystem playerHealth = other.GetComponent<SC_PlayerHealthSystem>();
            if (playerHealth != null)
            {
                float damageAmount = playerHealth.isBlocking ? 0f : playerHealth.maxHealth * 0.10f;

                if (damageAmount <= 0f)
                    SC_CombatFeedback.Instance?.PlayShieldBlock(transform.position);
                else
                    playerHealth.TakeDamage(damageAmount);
            }

            // safer return
            EnemyProjectileAutoDisable auto = GetComponent<EnemyProjectileAutoDisable>();
            if (auto != null)
                auto.ReturnToPool();
            else
                gameObject.SetActive(false);
        }
    }
}

