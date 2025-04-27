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
        // Update the position of the health bar
        transform.position = Camera.main.WorldToScreenPoint(target.position);

        // Calculate the forward direction of the camera
        Vector3 cameraForward = Camera.main.transform.forward;
        Vector3 dirToTarget = (target.position - Camera.main.transform.position).normalized;

        // Calculate the angle between the camera's forward direction and the direction to the target
        float angle = Vector3.Angle(cameraForward, dirToTarget);

        // Check if the target is within the camera's field of view
        bool isVisible = angle < 60f; // You can adjust to 70, 80, etc., if needed

        // Show or hide the health bar based on visibility
        if (canvasGroup != null)
            canvasGroup.alpha = isVisible ? 1f : 0f;
    }


    public void SetHealth(float percent)
    {
        if (fillImage != null)
            fillImage.fillAmount = percent;
    }
}
