using UnityEngine;
using UnityEngine.EventSystems;

public enum MobileInputAction
{
    MoveForward = 0,
    TurnLeft = 1,
    TurnRight = 2,
    Aim = 3,
    Shield = 4,
    Shoot = 5
}

// Forwards hold/release state from a UI element to SC_MobileInputController.
public class SC_MobileTouchButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
{
    public MobileInputAction action;

    public void OnPointerDown(PointerEventData eventData)
    {
        SetPressed(true);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        SetPressed(false);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        SetPressed(false);
    }

    private void OnDisable()
    {
        SetPressed(false);
    }

    private void SetPressed(bool pressed)
    {
        if (SC_MobileInputController.Instance != null)
            SC_MobileInputController.Instance.SetPressed(action, pressed);
    }
}
