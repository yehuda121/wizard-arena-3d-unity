using UnityEngine;

public class EnemyProjectileAutoDisable : MonoBehaviour
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
