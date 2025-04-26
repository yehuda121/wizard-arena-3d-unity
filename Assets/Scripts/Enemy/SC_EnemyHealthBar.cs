//using UnityEngine;
//using UnityEngine.UI;

//public class SC_EnemyHealthBar : MonoBehaviour
//{
//    public Image fillImage;        // Reference to the image component that fills the bar
//    public Transform target;       // The Transform (anchor point) the bar should follow

//    void Update()
//    {
//        // Keep the health bar positioned above the enemy on screen
//        if (target != null)
//        {
//            transform.position = Camera.main.WorldToScreenPoint(target.position);
//        }
//    }

//    // Sets the health percentage visually on the bar
//    public void SetHealth(float percent)
//    {
//        if (fillImage != null)
//            fillImage.fillAmount = percent;
//    }
//}
using UnityEngine;
using UnityEngine.UI;

public class SC_EnemyHealthBar : MonoBehaviour
{
    public Image fillImage;        // Reference to the image component that fills the bar
    public Transform target;       // The Transform (anchor point) the bar should follow
    private CanvasGroup canvasGroup;

    void Start()
    {
        // מוסיפים CanvasGroup כדי לשלוט על שקיפות
        canvasGroup = gameObject.AddComponent<CanvasGroup>();
    }

    void Update()
    {
        if (target == null)
            return;

        // מיקום הפס
        transform.position = Camera.main.WorldToScreenPoint(target.position);

        // חישוב כיוון המצלמה
        Vector3 cameraForward = Camera.main.transform.forward;
        Vector3 dirToTarget = (target.position - Camera.main.transform.position).normalized;

        // חישוב הזווית
        float angle = Vector3.Angle(cameraForward, dirToTarget);

        // האם בתוך שדה ראייה
        bool isVisible = angle < 60f; // תוכל לשנות ל-70, 80 לפי הצורך

        // הצגה/הסתרה
        if (canvasGroup != null)
            canvasGroup.alpha = isVisible ? 1f : 0f;
    }

    public void SetHealth(float percent)
    {
        if (fillImage != null)
            fillImage.fillAmount = percent;
    }
}
