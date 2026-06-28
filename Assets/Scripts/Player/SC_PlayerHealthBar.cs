
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SC_PlayerHealthBar : MonoBehaviour
{
    public Image fillImage;

    private Coroutine flashRoutine;
    private Color defaultFillColor = Color.white;

    void Awake()
    {
        if (fillImage != null)
            defaultFillColor = fillImage.color;
    }

    public void SetHealth(float percent)
    {
        if (fillImage != null)
            fillImage.fillAmount = percent;
    }

    public void FlashDamage()
    {
        if (fillImage == null)
            return;

        if (flashRoutine != null)
            StopCoroutine(flashRoutine);

        flashRoutine = StartCoroutine(FlashRoutine());
    }

    private IEnumerator FlashRoutine()
    {
        fillImage.color = new Color(1f, 0.25f, 0.25f, 1f);
        yield return new WaitForSeconds(0.12f);
        fillImage.color = defaultFillColor;
        flashRoutine = null;
    }
}
