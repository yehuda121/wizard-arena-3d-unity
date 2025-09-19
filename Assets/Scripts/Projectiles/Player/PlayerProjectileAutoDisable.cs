using UnityEngine;

public class PlayerProjectileAutoDisable : MonoBehaviour
{
    private float lifeTime = 3f;
    private bool isReturned = false;

    void OnEnable()
    {
        isReturned = false;
        Invoke(nameof(Disable), lifeTime);
    }

    void OnDisable()
    {
        CancelInvoke();
    }

    public void ReturnToPool()
    {
        if (!isReturned)
        {
            isReturned = true;
            if (PlayerProjectilePool.Instance != null)
                PlayerProjectilePool.Instance.ReturnProjectile(gameObject);
            else
                gameObject.SetActive(false);
        }
    }

    private void Disable()
    {
        ReturnToPool();
    }
}
