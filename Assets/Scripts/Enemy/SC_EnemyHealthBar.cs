using UnityEngine;
using UnityEngine.UI;

// This script controls the health bar that appears above enemy heads.
// It follows the enemy on screen and hides the bar when not visible to the camera.
public class SC_EnemyHealthBar : MonoBehaviour
{
    public Image fillImage;        // Reference to the fill image inside the health bar
    public Transform target;       // The world-space anchor the bar should follow (usually above the enemy's head)

    private CanvasGroup canvasGroup; // Used to control visibility of the bar via transparency

    void Start()
    {
        // Add CanvasGroup to allow fading in/out
        canvasGroup = gameObject.AddComponent<CanvasGroup>();
    }

    void Update()
    {
        // If target is gone or inactive — hide the bar
        if (target == null || !target.gameObject.activeInHierarchy)
        {
            canvasGroup.alpha = 0f;
            return;
        }

        // Update the screen position of the bar based on world position of the anchor
        transform.position = Camera.main.WorldToScreenPoint(target.position);

        // Calculate the direction from camera to the target
        Vector3 cameraForward = Camera.main.transform.forward;
        Vector3 dirToTarget = (target.position - Camera.main.transform.position).normalized;

        // Get the angle between camera forward and the target direction
        float angle = Vector3.Angle(cameraForward, dirToTarget);

        // If enemy is in front of camera (within field of view) — show the bar
        bool isVisible = angle < 60f;

        if (canvasGroup != null)
            canvasGroup.alpha = isVisible ? 1f : 0f;
    }

    // Called when health changes (value between 0–1)
    public void SetHealth(float percent)
    {
        if (fillImage != null)
            fillImage.fillAmount = percent;
    }
}
