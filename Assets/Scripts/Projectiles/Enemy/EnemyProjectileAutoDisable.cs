using UnityEngine;

public class EnemyProjectileAutoDisable : MonoBehaviour
{
    private float lifeTime = 6f;

    void OnEnable()
    {
        Invoke("Disable", lifeTime);
    }

    void Disable()
    {
        gameObject.SetActive(false);
    }
}
