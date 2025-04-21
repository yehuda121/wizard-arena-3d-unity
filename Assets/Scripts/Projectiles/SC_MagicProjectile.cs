//// This script controls projectile movement and collision behavior.
//// It moves forward and deals damage to enemies on impact, then disables itself.

//using UnityEngine;

//public class SC_MagicProjectile : MonoBehaviour
//{
//    public float speed = 20f;
//    public float damage = 25f;

//    void Update()
//    {
//        transform.Translate(Vector3.forward * speed * Time.deltaTime);
//    }

//    void OnTriggerEnter(Collider other)
//    {
//        if (other.CompareTag("Enemy"))
//        {
//            SC_HealthSystem health = other.GetComponent<SC_HealthSystem>();
//            if (health != null)
//            {
//                health.TakeDamage(damage);
//            }

//            gameObject.SetActive(false); // לא הורסים בגלל Object Pool
//        }
//        else if (!other.CompareTag("Player"))
//        {
//            gameObject.SetActive(false); // כבה גם בפגיעה בקיר וכו'
//        }
//    }
//}
// This script handles collision detection and damage dealing for a magic projectile.
// The projectile is moved externally (via Rigidbody velocity), and this script only disables it on impact.

using UnityEngine;

public class SC_MagicProjectile : MonoBehaviour
{
    public float damage = 25f;

    private void OnTriggerEnter(Collider other)
    {
        // If the projectile hits an enemy
        if (other.CompareTag("Enemy"))
        {
            // Try to get the enemy's health system component
            SC_HealthSystem health = other.GetComponent<SC_HealthSystem>();
            if (health != null)
            {
                // Deal damage to the enemy
                health.TakeDamage(damage);
            }

            // Disable the projectile (don't destroy, because we are using object pooling)
            gameObject.SetActive(false);
        }
        // If the projectile hits something else (but not the player)
        else if (!other.CompareTag("Player"))
        {
            // Disable the projectile on any other collision like a wall
            gameObject.SetActive(false);
        }
    }
}
