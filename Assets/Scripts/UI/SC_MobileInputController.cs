using UnityEngine;
using UnityEngine.UI;

// Manages on-screen mobile controls visibility and shared touch input state.
public class SC_MobileInputController : MonoBehaviour
{
    public static SC_MobileInputController Instance { get; private set; }

    private const string PrefsKey = "ShowMobileControls";
    private const int SmallScreenWidthThreshold = 1024;

    [SerializeField] private GameObject mobileControlsRoot;
    [SerializeField] private Toggle showMobileControlsToggle;

    public bool MoveForwardPressed { get; private set; }
    public bool TurnLeftPressed { get; private set; }
    public bool TurnRightPressed { get; private set; }
    public bool AimPressed { get; private set; }
    public bool ShieldPressed { get; private set; }
    public bool ShootPressed { get; private set; }

    public bool AreControlsVisible =>
        mobileControlsRoot != null && mobileControlsRoot.activeSelf;

    private int lastScreenWidth;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    void OnDestroy()
    {
        if (Instance == this)
            Instance = null;
    }

    void Start()
    {
        if (showMobileControlsToggle == null)
        {
            GameObject toggleObject = GameObject.Find("ShowMobileControlsToggle");
            if (toggleObject != null)
                showMobileControlsToggle = toggleObject.GetComponent<Toggle>();
        }

        lastScreenWidth = Screen.width;
        ApplyVisibilityPreference();
        SyncToggleWithoutNotify();

        if (showMobileControlsToggle != null)
        {
            showMobileControlsToggle.onValueChanged.RemoveListener(SetUserShowMobileControls);
            showMobileControlsToggle.onValueChanged.AddListener(SetUserShowMobileControls);
        }
    }

    void Update()
    {
        if (!PlayerPrefs.HasKey(PrefsKey) && Screen.width != lastScreenWidth)
        {
            lastScreenWidth = Screen.width;
            ApplyVisibilityPreference();
            SyncToggleWithoutNotify();
        }
    }

    public void SetPressed(MobileInputAction action, bool pressed)
    {
        switch (action)
        {
            case MobileInputAction.MoveForward:
                MoveForwardPressed = pressed;
                break;
            case MobileInputAction.TurnLeft:
                TurnLeftPressed = pressed;
                break;
            case MobileInputAction.TurnRight:
                TurnRightPressed = pressed;
                break;
            case MobileInputAction.Aim:
                AimPressed = pressed;
                break;
            case MobileInputAction.Shield:
                ShieldPressed = pressed;
                break;
            case MobileInputAction.Shoot:
                ShootPressed = pressed;
                break;
        }
    }

    public void SetUserShowMobileControls(bool show)
    {
        PlayerPrefs.SetInt(PrefsKey, show ? 1 : 0);
        PlayerPrefs.Save();
        ApplyVisibilityPreference();
    }

    private bool ShouldShowControls()
    {
        if (PlayerPrefs.HasKey(PrefsKey))
            return PlayerPrefs.GetInt(PrefsKey, 0) == 1;

        return Application.isMobilePlatform || Screen.width < SmallScreenWidthThreshold;
    }

    private void ApplyVisibilityPreference()
    {
        if (mobileControlsRoot != null)
            mobileControlsRoot.SetActive(ShouldShowControls());
    }

    private void SyncToggleWithoutNotify()
    {
        if (showMobileControlsToggle != null)
            showMobileControlsToggle.SetIsOnWithoutNotify(ShouldShowControls());
    }
}
