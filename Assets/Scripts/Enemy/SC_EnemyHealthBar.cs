using UnityEngine;
using UnityEngine.UI;

public class SC_EnemyHealthBar : MonoBehaviour
{
    public Image fillImage;        // Reference to the image component that fills the bar
    public Transform target;       // The Transform (anchor point) the bar should follow

    void Update()
    {
        // Keep the health bar positioned above the enemy on screen
        if (target != null)
        {
            transform.position = Camera.main.WorldToScreenPoint(target.position);
        }
    }

    // Sets the health percentage visually on the bar
    public void SetHealth(float percent)
    {
        if (fillImage != null)
            fillImage.fillAmount = percent;
    }
}
