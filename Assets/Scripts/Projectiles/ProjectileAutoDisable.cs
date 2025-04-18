// This script automatically disables a projectile after a fixed time.
// Used to clean up projectiles that do not hit anything.

using UnityEngine;

public class ProjectileAutoDisable : MonoBehaviour
{
    private float lifeTime = 2f;

    void OnEnable()
    {
        Invoke("Disable", lifeTime);
    }

    void Disable()
    {
        gameObject.SetActive(false);
    }
}
