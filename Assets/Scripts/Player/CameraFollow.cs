using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour
{
    public Transform target;  // The follow target (CameraFollowTarget)
    public Vector3 normalOffset = new Vector3(0, 4, -4); // third-person
    public Vector3 aimOffset = new Vector3(0, 4f, 6f); // first-person
    public float smoothSpeed = 10f;

    [Header("Damage Shake")]
    [SerializeField] private float damageShakeAmplitude = 0.12f;
    [SerializeField] private float damageShakeDuration = 0.12f;

    private bool isAiming = false;
    private Coroutine shakeRoutine;
    private Vector3 shakeOffset = Vector3.zero;

    void Update()
    {
        SC_MobileInputController mobileInput = SC_MobileInputController.Instance;
        bool mobileAim = mobileInput != null && mobileInput.AimPressed;

        // Enter/Exit aim mode by holding Down Arrow or mobile aim button
        isAiming = Input.GetKey(KeyCode.DownArrow) || mobileAim;
    }

    void LateUpdate()
    {
        Vector3 desiredOffset = isAiming ? aimOffset : normalOffset;
        Vector3 desiredPosition = target.position + target.rotation * desiredOffset;

        transform.position = Vector3.Lerp(transform.position, desiredPosition + shakeOffset, smoothSpeed * Time.deltaTime);
        transform.rotation = Quaternion.Lerp(transform.rotation, target.rotation, smoothSpeed * Time.deltaTime);
    }

    public void PlayDamageShake()
    {
        if (damageShakeAmplitude <= 0f || damageShakeDuration <= 0f)
            return;

        if (shakeRoutine != null)
            StopCoroutine(shakeRoutine);

        shakeRoutine = StartCoroutine(DamageShakeRoutine());
    }

    private IEnumerator DamageShakeRoutine()
    {
        float elapsed = 0f;

        while (elapsed < damageShakeDuration)
        {
            elapsed += Time.deltaTime;
            float strength = 1f - (elapsed / damageShakeDuration);
            shakeOffset = Random.insideUnitSphere * damageShakeAmplitude * strength;
            shakeOffset.y *= 0.35f;
            yield return null;
        }

        shakeOffset = Vector3.zero;
        shakeRoutine = null;
    }

    public bool IsAiming()
    {
        return isAiming;
    }
}
