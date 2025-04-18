// This script controls projectile movement and collision behavior.
// It moves forward and deals damage to enemies on impact, then disables itself.

using UnityEngine;

public class SC_MagicProjectile : MonoBehaviour
{
    public float speed = 20f;
    public float damage = 25f;

    void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            SC_HealthSystem health = other.GetComponent<SC_HealthSystem>();
            if (health != null)
            {
                health.TakeDamage(damage);
            }

            gameObject.SetActive(false); // לא הורסים בגלל Object Pool
        }
        else if (!other.CompareTag("Player"))
        {
            gameObject.SetActive(false); // כבה גם בפגיעה בקיר וכו'
        }
    }
}
