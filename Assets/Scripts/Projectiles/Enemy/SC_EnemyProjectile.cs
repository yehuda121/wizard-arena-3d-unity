using UnityEngine;

// Handles projectiles fired by regular enemies
public class SC_EnemyProjectile : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // Enemy shot and hit the player
        if (other.CompareTag("Player"))
        {
            SC_PlayerHealthSystem playerHealth = other.GetComponent<SC_PlayerHealthSystem>();
            if (playerHealth != null)
            {
                float damageAmount = playerHealth.isBlocking ? 0f : playerHealth.maxHealth * 0.10f;
                playerHealth.TakeDamage(damageAmount);
            }

            gameObject.SetActive(false);
        }
    }
}

