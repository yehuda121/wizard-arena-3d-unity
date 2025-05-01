// This script automatically disables a projectile after a fixed time.
// Used to clean up projectiles that do not hit anything.

using UnityEngine;

public class PlayerProjectileAutoDisable : MonoBehaviour
{
    private float lifeTime = 2.5f;

    void OnEnable()
    {
        Invoke("Disable", lifeTime);
    }

    void Disable()
    {
        gameObject.SetActive(false);
    }
}
