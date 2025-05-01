using UnityEngine;

public class LimitFrameRate : MonoBehaviour
{
    void Awake()
    {
        Application.targetFrameRate = 60; // מגביל ל־60 פריימים לשנייה
    }
}
