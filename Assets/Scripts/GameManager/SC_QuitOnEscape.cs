using UnityEngine;
using UnityEngine.UI;

public class SC_QuitPopupManager : MonoBehaviour
{
    [Header("UI References")]
    public GameObject quitPopupPanel;
    public Button yesButton;
    public Button noButton;

    private bool isPopupOpen = false;

    void Start()
    {
        quitPopupPanel.SetActive(false);

        yesButton.onClick.AddListener(QuitGame);
        noButton.onClick.AddListener(ClosePopup);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !isPopupOpen)
        {
            OpenPopup();
        }
    }

    void OpenPopup()
    {
        quitPopupPanel.SetActive(true);
        isPopupOpen = true;
    }

    void ClosePopup()
    {
        quitPopupPanel.SetActive(false);
        isPopupOpen = false;
    }

    void QuitGame()
    {
#if UNITY_EDITOR
        quitPopupPanel.SetActive(false);
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
