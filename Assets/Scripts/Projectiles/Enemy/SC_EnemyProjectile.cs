using UnityEngine;

public class SC_EnemyProjectile : MonoBehaviour
{
    public float damage = 10f;
    private Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void OnEnable()
    {
        if (rb != null)
        {
            rb.isKinematic = false;
            rb.detectCollisions = true;
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            SC_Player player = collision.gameObject.GetComponent<SC_Player>();
            if (player != null)
            {
                player.TakeDamage(damage);
            }
        }
        CancelInvoke();
        Disable();
    }
    //void OnTriggerEnter(Collider other)
    //{
    //    Debug.Log("Trigger detected with: " + other.gameObject.name + " | Tag: " + other.gameObject.tag);

    //    if (other.CompareTag("Player"))
    //    {
    //        SC_Player player = other.GetComponent<SC_Player>();
    //        if (player != null)
    //        {
    //            player.TakeDamage(damage);
    //        }

    //        CancelInvoke();
    //        Disable();
    //    }
    //    else
    //    {
    //        CancelInvoke();
    //        Disable();
    //    }
    //}

    void Disable()
    {
        if (rb != null)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.isKinematic = true;
            rb.detectCollisions = false;
        }

        gameObject.SetActive(false);
    }
}
