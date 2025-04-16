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
