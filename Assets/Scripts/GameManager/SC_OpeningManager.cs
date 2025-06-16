using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class SC_OpeningManager : MonoBehaviour
{
    [Header("UI References")]
    public GameObject settingsPanel;     // The settings UI panel to show after the video
    public Button playButton;            // The button that will trigger loading the next scene

    [Header("Video Settings")]
    public VideoPlayer videoPlayer;      // The video player component
    public TMP_Dropdown difficultyDropdown;  // TMP

    private bool settingsShown = false;  // Ensures settings panel is only shown once

    void Start()
    {
        bool skip = PlayerPrefs.GetInt("SkipOpeningVideo", 0) == 1;
        if (skip)
        {
            if (difficultyDropdown != null)
            {
                difficultyDropdown.value = PlayerPrefs.GetInt("SelectedDifficulty", 0);
            }

            PlayerPrefs.SetInt("SkipOpeningVideo", 0); 
            ShowSettings();
            return;
        }

        // Hide the settings panel at start
        settingsPanel.SetActive(false);

        // Connect to video end event
        if (videoPlayer != null)
        {
            videoPlayer.loopPointReached += OnVideoEnd;
        }
        else
        {
            Debug.LogWarning("Video Player is not assigned!");
        }

        // Safely connect the Play button click event
        if (playButton != null)
        {
            playButton.onClick.RemoveAllListeners();           // Clear any old/broken listeners
            playButton.onClick.AddListener(OnPlayButtonClick); // Assign click event
        }
        else
        {
            Debug.LogWarning("Play button is not assigned!");
        }
    }

    // Called automatically when the video finishes playing
    void OnVideoEnd(VideoPlayer vp)
    {
        ShowSettings();
    }

    public void SkipVideo()
    {
        if (difficultyDropdown != null)
        {
            difficultyDropdown.value = PlayerPrefs.GetInt("SelectedDifficulty", 0);
        }

        ShowSettings();
    }


    // Show the settings panel (only once)
    void ShowSettings()
    {
        if (!settingsShown)
        {
            settingsShown = true;
            settingsPanel.SetActive(true);
        }
    }

    // Called when the play button is clicked
    public void OnPlayButtonClick()
    {
        if (difficultyDropdown != null)
        {
            PlayerPrefs.SetInt("SelectedDifficulty", difficultyDropdown.value);
            PlayerPrefs.Save();
        }

        Debug.Log("Play button clicked!");
        SceneManager.LoadScene("MainArena");
    }
}
