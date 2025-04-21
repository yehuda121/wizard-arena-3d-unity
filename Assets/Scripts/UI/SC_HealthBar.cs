using UnityEngine;
using UnityEngine.UI;

public class SC_HealthBar : MonoBehaviour
{
    public Image fillImage;
    public Transform target; // HealthBarAnchor

    void Update()
    {
        transform.position = Camera.main.WorldToScreenPoint(target.position);
    }

    public void SetHealth(float percent)
    {
        fillImage.fillAmount = percent;
    }
}
