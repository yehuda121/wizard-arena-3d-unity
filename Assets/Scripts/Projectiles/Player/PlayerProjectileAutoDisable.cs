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
        if (isReturned)
            return;

        isReturned = true;
        CancelInvoke(nameof(Disable));

        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        if (PlayerProjectilePool.Instance != null)
            PlayerProjectilePool.Instance.ReturnProjectile(gameObject);
        else
            gameObject.SetActive(false);
    }

    private void Disable()
    {
        ReturnToPool();
    }
}
