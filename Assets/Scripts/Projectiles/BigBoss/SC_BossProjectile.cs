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
                if (playerHealth.isBlocking)
                {
                    // Player is blocking → take 5% damage (not 0!)
                    playerHealth.TakeDamage(playerHealth.maxHealth * 0.05f);
                }
                else
                {
                    // No blocking → take 35% damage
                    playerHealth.TakeDamage(playerHealth.maxHealth * 0.35f);
                }
            }
        }

        Destroy(gameObject); // Or SetActive(false) if pooling
    }
}
