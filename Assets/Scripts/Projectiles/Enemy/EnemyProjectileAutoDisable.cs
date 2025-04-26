using UnityEngine;

public class EnemyProjectileAutoDisable : MonoBehaviour
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
