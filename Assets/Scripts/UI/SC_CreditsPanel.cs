using UnityEngine;
using UnityEngine.UI;

public class SC_CreditsPanel : MonoBehaviour
{
    public GameObject creditsScrollView;
    public ScrollRect scrollRect;

    public void ShowCredits()
    {
        creditsScrollView.SetActive(true);
        ResetScroll();
    }

    public void HideCredits()
    {
        creditsScrollView.SetActive(false);
    }

    private void ResetScroll()
    {
        if (scrollRect != null)
        {
            scrollRect.verticalNormalizedPosition = 1f;
        }
    }
}
