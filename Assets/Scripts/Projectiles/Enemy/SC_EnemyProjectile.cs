////using UnityEngine;

////public class SC_EnemyProjectile : MonoBehaviour
////{
////    public float damage = 10f;
////    private Rigidbody rb;

////    void Awake()
////    {
////       Debug.Log("awaked");
////       rb = GetComponent<Rigidbody>();
////    }

////    void OnEnable()
////    {
////        if (rb != null)
////        {
////            rb.isKinematic = false;
////            rb.detectCollisions = true;
////            rb.velocity = Vector3.zero;
////            rb.angularVelocity = Vector3.zero;
////        }
////    }

////    void OnCollisionEnter(Collision collision)
////    {
////        Debug.Log("is colide");
////        if (collision.gameObject.CompareTag("Player"))
////        {
////            SC_Player player = collision.gameObject.GetComponent<SC_Player>();
////            if (player != null)
////            {
////                player.TakeDamage(damage);
////            }
////        }
////        CancelInvoke();
////        Disable();
////    }
////    //void OnTriggerEnter(Collider other)
////    //{
////    //    Debug.Log("Trigger detected with: " + other.gameObject.name + " | Tag: " + other.gameObject.tag);

////    //    if (other.CompareTag("Player"))
////    //    {
////    //        SC_Player player = other.GetComponent<SC_Player>();
////    //        if (player != null)
////    //        {
////    //            player.TakeDamage(damage);
////    //        }

////    //        CancelInvoke();
////    //        Disable();
////    //    }
////    //    else
////    //    {
////    //        CancelInvoke();
////    //        Disable();
////    //    }
////    //}

////    void Disable()
////    {
////        if (rb != null)
////        {
////            rb.velocity = Vector3.zero;
////            rb.angularVelocity = Vector3.zero;
////            rb.isKinematic = true;
////            rb.detectCollisions = false;
////        }

////        gameObject.SetActive(false);
////    }
////}
//using UnityEngine;

//public class SC_EnemyProjectile : MonoBehaviour
//{
//    public float damage = 10f;
//    private Rigidbody rb;

//    void Awake()
//    {
//        rb = GetComponent<Rigidbody>();
//    }

//    void OnEnable()
//    {
//        if (rb != null)
//        {
//            rb.isKinematic = false;
//            rb.detectCollisions = true;
//            rb.velocity = Vector3.zero;
//            rb.angularVelocity = Vector3.zero;
//        }
//    }

//    //void OnCollisionEnter(Collision collision)
//    //{
//    //    Debug.Log("enemy projectile hit");
//    //    if (collision.gameObject.CompareTag("Player"))
//    //    {
//    //        SC_PlayerHealthSystem playerHealth = collision.gameObject.GetComponent<SC_PlayerHealthSystem>();
//    //        if (playerHealth != null)
//    //        {
//    //            playerHealth.TakeDamage(damage);
//    //        }
//    //    }

//    //    Disable();
//    //}
//    void OnTriggerEnter(Collider other)
//    {
//        if (other.CompareTag("Player"))
//        {
//            SC_PlayerHealthSystem playerHealth = other.GetComponent<SC_PlayerHealthSystem>();
//            if (playerHealth != null)
//            {
//                playerHealth.TakeDamage(damage);
//            }
//        }

//        Disable();
//    }


//    void Disable()
//    {
//        if (rb != null)
//        {
//            rb.velocity = Vector3.zero;
//            rb.angularVelocity = Vector3.zero;
//            rb.isKinematic = true;
//            rb.detectCollisions = false;
//        }

//        gameObject.SetActive(false);
//    }
//}

using UnityEngine;

public class SC_EnemyProjectile : MonoBehaviour
{
    // This value is no longer used directly but can be kept for flexibility
    public float damage = 25f;

    public enum ShooterType { Player, Enemy }
    public ShooterType shooter;

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("player projectile trigered");
        //Player shot and hit an Enemy
        if (shooter == ShooterType.Player && other.CompareTag("Enemy"))
        {
            SC_EnemyHealthSystem enemyHealth = other.GetComponent<SC_EnemyHealthSystem>();
            if (enemyHealth != null)
            {
                float percent = 0.25f; // 25% damage
                enemyHealth.TakeDamage(enemyHealth.maxHealth * percent);
            }

            gameObject.SetActive(false); // always disable after valid hit
        }

        // Enemy shot and hit the Player
        else if (shooter == ShooterType.Enemy && other.CompareTag("Player"))
        {
            SC_PlayerHealthSystem playerHealth = other.GetComponent<SC_PlayerHealthSystem>();
            if (playerHealth != null)
            {
                float damageAmount = 10f;
                playerHealth.TakeDamage(damageAmount);
            }
            gameObject.SetActive(false); // always disable after valid hit
        }

        // No action if projectile hits another projectile or its own shooter-type target
    }
}
