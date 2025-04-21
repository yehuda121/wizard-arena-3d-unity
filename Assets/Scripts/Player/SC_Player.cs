using UnityEngine;

public class SC_Player : MonoBehaviour
{
    public float maxHealth = 100f;
    private float currentHealth;

    public SC_HealthBar healthBar;

    void Start()
    {
        currentHealth = maxHealth;

        // ????? ?? ????? ????? (??? PlayerHealthBar)
        GameObject healthBarObj = GameObject.Find("PlayerHealthBar");
        if (healthBarObj != null)
        {
            healthBar = healthBarObj.GetComponent<SC_HealthBar>();
            healthBar.target = GameObject.Find("HealthBarAnchor").transform; // ????? ??? ??????
        }
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        float percent = currentHealth / maxHealth;

        if (healthBar != null)
            healthBar.SetHealth(percent);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Player Died!");
        // ???? ?????? Game Over ??? ?????
    }
}
