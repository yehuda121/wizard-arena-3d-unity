
using UnityEngine;
using UnityEngine.UI;

public class SC_PlayerHealthBar : MonoBehaviour
{
    public Image fillImage;

    public void SetHealth(float percent)
    {
        if (fillImage != null)
            fillImage.fillAmount = percent;
    }
}
