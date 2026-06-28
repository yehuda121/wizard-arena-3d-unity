using UnityEngine;

public class LimitFrameRate : MonoBehaviour
{
    void Awake()
    {
        Application.targetFrameRate = 60; // 60 fps
    }
}
