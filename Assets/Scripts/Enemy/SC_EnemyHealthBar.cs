
using UnityEngine;
using UnityEngine.UI;

public class SC_EnemyHealthBar : MonoBehaviour
{
    public Image fillImage;       
    public Transform target;      

    private CanvasGroup canvasGroup;

    void Start()
    {
        canvasGroup = gameObject.AddComponent<CanvasGroup>();

        Canvas canvas = GetComponent<Canvas>();
        if (canvas != null && canvas.renderMode == RenderMode.WorldSpace && canvas.worldCamera == null)
        {
            canvas.worldCamera = Camera.main;
        }
    }

    void LateUpdate()
    {
        if (target == null || !target.gameObject.activeInHierarchy)
        {
            canvasGroup.alpha = 0f;
            return;
        }

        // rotate to face the camera
        transform.forward = Camera.main.transform.forward;

        // Show or hide based on angle from camera
        Vector3 cameraForward = Camera.main.transform.forward;
        Vector3 dirToTarget = (target.position - Camera.main.transform.position).normalized;
        float angle = Vector3.Angle(cameraForward, dirToTarget);

        canvasGroup.alpha = angle < 90f ? 1f : 0f;
    }

    public void SetHealth(float percent)
    {
        if (fillImage != null)
            fillImage.fillAmount = percent;
    }
}
