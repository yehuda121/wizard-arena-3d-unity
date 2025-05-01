using UnityEngine;

public class EnemyProjectileAutoDisable : MonoBehaviour
{
    private float lifeTime = 3f;

    void OnEnable()
    {
        Invoke("Disable", lifeTime);
    }

    void Disable()
    {
        gameObject.SetActive(false);
    }
}
