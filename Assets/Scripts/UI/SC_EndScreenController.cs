using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

// Game Over / Victory panels with Restart and Main Menu (no forced auto-exit).
public class SC_EndScreenController : MonoBehaviour
{
    public static SC_EndScreenController Instance { get; private set; }

    [SerializeField] private Canvas gameOverCanvas;
    [SerializeField] private Canvas victoryCanvas;
    [SerializeField] private TMP_Text gameOverTitleText;
    [SerializeField] private TMP_Text victoryTitleText;

    private bool gameOverUiBuilt;
    private bool victoryUiBuilt;
    private bool endScreenShowing;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        ResolveReferences();
    }

    void Start()
    {
        if (gameOverCanvas != null)
            gameOverCanvas.gameObject.SetActive(false);

        if (victoryCanvas != null)
            victoryCanvas.gameObject.SetActive(false);
    }

    void OnDestroy()
    {
        if (Instance == this)
            Instance = null;
    }

    public void ShowGameOver()
    {
        if (endScreenShowing)
            return;

        endScreenShowing = true;
        BuildGameOverUiIfNeeded();

        if (gameOverCanvas != null)
        {
            gameOverCanvas.sortingOrder = 200;
            gameOverCanvas.gameObject.SetActive(true);
        }

        Time.timeScale = 0f;
    }

    public void ShowVictory()
    {
        if (endScreenShowing)
            return;

        endScreenShowing = true;
        BuildVictoryUiIfNeeded();

        if (victoryCanvas != null)
        {
            victoryCanvas.sortingOrder = 200;
            victoryCanvas.gameObject.SetActive(true);
        }

        Time.timeScale = 0f;
    }

    public void Restart()
    {
        Time.timeScale = 1f;
        endScreenShowing = false;

        SC_GameManager gameManager = FindObjectOfType<SC_GameManager>();
        if (gameManager != null)
            gameManager.RestartGame();
    }

    public void MainMenu()
    {
        Time.timeScale = 1f;
        endScreenShowing = false;

        SC_GameManager gameManager = FindObjectOfType<SC_GameManager>();
        if (gameManager != null)
            gameManager.ReturnToMainMenu();
    }

    private void ResolveReferences()
    {
        if (gameOverCanvas == null)
        {
            GameObject go = GameObject.Find("GameOverCanvas");
            if (go != null)
                gameOverCanvas = go.GetComponent<Canvas>();
        }

        if (victoryCanvas == null)
        {
            GameObject go = GameObject.Find("VictoryCanvas");
            if (go != null)
                victoryCanvas = go.GetComponent<Canvas>();
        }

        if (gameOverTitleText == null)
        {
            GameObject go = GameObject.Find("GameOverText");
            if (go != null)
                gameOverTitleText = go.GetComponent<TMP_Text>();
        }

        if (victoryTitleText == null)
        {
            GameObject victoryCanvasGo = GameObject.Find("VictoryCanvas");
            if (victoryCanvasGo != null)
            {
                Transform victoryText = victoryCanvasGo.transform.Find("VictoryText");
                if (victoryText != null)
                    victoryTitleText = victoryText.GetComponent<TMP_Text>();
            }
        }
    }

    private void BuildGameOverUiIfNeeded()
    {
        if (gameOverUiBuilt || gameOverCanvas == null)
            return;

        gameOverUiBuilt = true;
        BuildScreenUi(
            gameOverCanvas.transform,
            gameOverTitleText,
            "GAME OVER",
            new Color(0.55f, 0.05f, 0.05f, 1f));
    }

    private void BuildVictoryUiIfNeeded()
    {
        if (victoryUiBuilt || victoryCanvas == null)
            return;

        victoryUiBuilt = true;
        BuildScreenUi(
            victoryCanvas.transform,
            victoryTitleText,
            "VICTORY!",
            new Color(0.95f, 0.85f, 0.2f, 1f));
    }

    private void BuildScreenUi(Transform canvasRoot, TMP_Text titleText, string title, Color titleColor)
    {
        TMP_FontAsset font = titleText != null ? titleText.font : null;

        GameObject overlay = CreateUiObject("Overlay", canvasRoot);
        RectTransform overlayRect = overlay.GetComponent<RectTransform>();
        StretchFull(overlayRect);
        Image overlayImage = overlay.AddComponent<Image>();
        overlayImage.color = new Color(0f, 0f, 0f, 0.72f);
        overlayImage.raycastTarget = true;

        if (titleText != null)
        {
            titleText.text = title;
            titleText.fontSize = 72;
            titleText.color = titleColor;
            titleText.alignment = TextAlignmentOptions.Center;
            titleText.gameObject.SetActive(true);

            RectTransform titleRect = titleText.rectTransform;
            titleRect.anchorMin = new Vector2(0.5f, 0.5f);
            titleRect.anchorMax = new Vector2(0.5f, 0.5f);
            titleRect.pivot = new Vector2(0.5f, 0.5f);
            titleRect.anchoredPosition = new Vector2(0f, 80f);
            titleRect.sizeDelta = new Vector2(700f, 120f);
        }

        CreateMenuButton("RestartButton", canvasRoot, font, "Restart", new Vector2(0f, -40f), Restart);
        CreateMenuButton("MainMenuButton", canvasRoot, font, "Main Menu", new Vector2(0f, -110f), MainMenu);
    }

    private void CreateMenuButton(
        string name,
        Transform parent,
        TMP_FontAsset font,
        string label,
        Vector2 anchoredPosition,
        UnityAction onClick)
    {
        GameObject buttonGo = CreateUiObject(name, parent);
        RectTransform rect = buttonGo.GetComponent<RectTransform>();
        rect.anchorMin = new Vector2(0.5f, 0.5f);
        rect.anchorMax = new Vector2(0.5f, 0.5f);
        rect.pivot = new Vector2(0.5f, 0.5f);
        rect.anchoredPosition = anchoredPosition;
        rect.sizeDelta = new Vector2(220f, 44f);

        Image image = buttonGo.AddComponent<Image>();
        image.color = new Color(0.18f, 0.18f, 0.18f, 0.92f);

        Button button = buttonGo.AddComponent<Button>();
        button.targetGraphic = image;
        button.onClick.AddListener(onClick);

        GameObject labelGo = CreateUiObject("Label", buttonGo.transform);
        RectTransform labelRect = labelGo.GetComponent<RectTransform>();
        StretchFull(labelRect);

        TextMeshProUGUI tmp = labelGo.AddComponent<TextMeshProUGUI>();
        tmp.text = label;
        tmp.font = font;
        tmp.fontSize = 24;
        tmp.alignment = TextAlignmentOptions.Center;
        tmp.color = Color.white;
        tmp.raycastTarget = false;
    }

    private static GameObject CreateUiObject(string name, Transform parent)
    {
        GameObject go = new GameObject(name, typeof(RectTransform));
        go.transform.SetParent(parent, false);
        go.layer = parent.gameObject.layer;
        return go;
    }

    private static void StretchFull(RectTransform rect)
    {
        rect.anchorMin = Vector2.zero;
        rect.anchorMax = Vector2.one;
        rect.offsetMin = Vector2.zero;
        rect.offsetMax = Vector2.zero;
        rect.pivot = new Vector2(0.5f, 0.5f);
    }
}
