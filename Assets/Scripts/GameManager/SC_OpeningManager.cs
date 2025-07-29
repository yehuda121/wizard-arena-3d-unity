using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System.Collections;


public class SC_OpeningManager : MonoBehaviour
{
    [Header("UI References")]
    public GameObject settingsPanel;     // The settings UI panel to show after the video
    public Button playButton;            // The button that will trigger loading the next scene
    public GameObject skipButton;        // The button to skip the video

    [Header("Video Settings")]
    public VideoPlayer videoPlayer;      // The video player component
    //public TMP_Dropdown difficultyDropdown;  // TMP

    public AudioSource openingMusic;

    private bool settingsShown = false;  // Ensures settings panel is only shown once

    void Start()
    {
        bool skip = PlayerPrefs.GetInt("SkipOpeningVideo", 0) == 1;
        if (skip)
        {
            PlayerPrefs.SetInt("SkipOpeningVideo", 0);
            ShowSettings();
            if (skipButton != null)
                skipButton.SetActive(false);
            return;
        }

        // Hide the settings panel at start
        settingsPanel.SetActive(false);

        // Hide the skip button at start
        //if (skipButton != null)
        //    skipButton.SetActive(false);

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

        // Show skip button only if video is playing
        StartCoroutine(ShowSkipIfVideoIsPlaying());
    }

    // Called automatically when the video finishes playing
    void OnVideoEnd(VideoPlayer vp)
    {
        if (skipButton != null)
            skipButton.SetActive(false); // Hide skip button when video ends
        ShowSettings();
    }

    public void SkipVideo()
    {
        skipButton?.SetActive(false); // Hide skip button after it's used

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
        if (openingMusic != null)
            openingMusic.Stop();

        //Debug.Log("Play button clicked!");
        SceneManager.LoadScene("MainArena");
    }

    // Coroutine to check if video is playing and show skip button
    private IEnumerator ShowSkipIfVideoIsPlaying()
    {
        yield return new WaitForSeconds(0.2f); // Wait a short moment for video to start

        if (videoPlayer != null && videoPlayer.isPlaying && skipButton != null)
        {
            skipButton.SetActive(true);
        }
    }
}
