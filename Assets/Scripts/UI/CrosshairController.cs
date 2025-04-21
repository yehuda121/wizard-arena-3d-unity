using UnityEngine;

public class CrosshairController : MonoBehaviour
{
    public GameObject crosshairUI;
    private CameraFollow cameraFollow;

    void Start()
    {
        cameraFollow = Camera.main.GetComponent<CameraFollow>();
        crosshairUI.SetActive(false);
    }

    void Update()
    {
        if (cameraFollow.IsAiming())
        {
            crosshairUI.SetActive(true);
        }
        else
        {
            crosshairUI.SetActive(false);
        }
    }
}
