using UnityEngine;

public class EnemyProjectileAutoDisable : MonoBehaviour
{
    private float lifeTime = 6f;
    private bool isReturned = false;

    void OnEnable()
    {
        isReturned = false; // reset every time the projectile is reused
        Invoke(nameof(Disable), lifeTime);
    }

    void OnDisable()
    {
        CancelInvoke(); // make sure no leftover invokes run
    }

    public void ReturnToPool()
    {
        if (!isReturned)
        {
            isReturned = true;
            if (EnemyProjectilePool.Instance != null)
                EnemyProjectilePool.Instance.ReturnProjectile(gameObject);
            else
                gameObject.SetActive(false);
        }
    }

    private void Disable()
    {
        ReturnToPool();
    }
}

