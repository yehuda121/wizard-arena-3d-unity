using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;  // The follow target (CameraFollowTarget)
    public Vector3 normalOffset = new Vector3(0, 2, -5); // third-person
    public Vector3 aimOffset = new Vector3(0, 1.6f, 0.2f); // first-person
    public float smoothSpeed = 10f;

    private bool isAiming = false;

    void Update()
    {
        // Enter/Exit aim mode by holding Down Arrow
        isAiming = Input.GetKey(KeyCode.DownArrow);
    }

    void LateUpdate()
    {
        Vector3 desiredOffset = isAiming ? aimOffset : normalOffset;
        Vector3 desiredPosition = target.position + target.rotation * desiredOffset;

        transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
        transform.rotation = Quaternion.Lerp(transform.rotation, target.rotation, smoothSpeed * Time.deltaTime);
    }

    public bool IsAiming()
    {
        return isAiming;
    }
}
