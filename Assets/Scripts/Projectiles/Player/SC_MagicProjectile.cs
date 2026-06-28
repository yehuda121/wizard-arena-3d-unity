using UnityEngine;

public class SC_MagicProjectile : MonoBehaviour
{
    private const float EnemyDamageFraction = 0.25f;
    private const float BossDamageFraction = 0.10f;

    [HideInInspector]
    public float damageMultiplier = 1f;

    private bool hasHit;
    private PlayerProjectileAutoDisable autoDisable;
    private Rigidbody projectileRigidbody;

    private void Awake()
    {
        autoDisable = GetComponent<PlayerProjectileAutoDisable>();
        projectileRigidbody = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        hasHit = false;
        damageMultiplier = 1f;

        foreach (ParticleSystem ps in GetComponentsInChildren<ParticleSystem>())
        {
            ps.Clear();
            ps.Play();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (hasHit)
            return;

        if (!other.CompareTag("Enemy") && !other.CompareTag("BossEnemy"))
        {
            SC_CombatFeedback.Instance?.PlayProjectileImpact(transform.position);
            DeactivateAfterHit();
            return;
        }

        Vector3 hitPoint = other.ClosestPoint(transform.position);

        if (other.CompareTag("BossEnemy"))
        {
            SC_BossEnemyHealthSystem bossHealth = other.GetComponent<SC_BossEnemyHealthSystem>();
            if (bossHealth != null)
            {
                bossHealth.TakeDamage(bossHealth.maxHealth * BossDamageFraction * damageMultiplier);
                SC_CombatFeedback.Instance?.PlayEnemyHit(hitPoint);
            }
        }
        else if (other.CompareTag("Enemy"))
        {
            SC_EnemyHealthSystem enemyHealth = other.GetComponent<SC_EnemyHealthSystem>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(enemyHealth.maxHealth * EnemyDamageFraction * damageMultiplier);
                SC_CombatFeedback.Instance?.PlayEnemyHit(hitPoint);
            }
        }

        DeactivateAfterHit();
    }

    private void DeactivateAfterHit()
    {
        if (hasHit)
            return;

        hasHit = true;

        if (projectileRigidbody != null)
        {
            projectileRigidbody.velocity = Vector3.zero;
            projectileRigidbody.angularVelocity = Vector3.zero;
        }

        if (autoDisable != null)
        {
            autoDisable.ReturnToPool();
            return;
        }

        if (PlayerProjectilePool.Instance != null)
            PlayerProjectilePool.Instance.ReturnProjectile(gameObject);
        else
            gameObject.SetActive(false);
    }
}
