using UnityEngine;

// Behavior for the boss projectile
public class SC_BossProjectile : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SC_PlayerHealthSystem playerHealth = other.GetComponent<SC_PlayerHealthSystem>();
            if (playerHealth != null)
            {
                // Always send full boss damage; shield handling is done inside TakeDamage
                float rawDamage = playerHealth.maxHealth * 0.35f;

                if (playerHealth.isBlocking)
                    SC_CombatFeedback.Instance?.PlayShieldBlock(transform.position);

                playerHealth.TakeDamage(rawDamage);
            }
        }

        // Disable the projectile (destroy or pooling)
        Destroy(gameObject);
        //gameObject.SetActive(false); // or Destroy(gameObject) if not using pooling
    }
}
