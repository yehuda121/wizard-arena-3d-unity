using UnityEngine;

public class SC_MusicManager : MonoBehaviour
{
    public AudioSource musicSource;

    public AudioClip musicEasy;
    public AudioClip musicMedium;
    public AudioClip musicHard;
    public AudioClip musicBoss;

    private DifficultyLevel lastDifficulty;
    private SC_GameManager gameManager;

    // Called at game start
    void Start()
    {
        gameManager = FindObjectOfType<SC_GameManager>();
        if (gameManager != null)
        {
            lastDifficulty = gameManager.currentDifficulty;
            PlayMusicForDifficulty(lastDifficulty);
        }
    }

    void Update()
    {
        // Check if the difficulty level has changed
        if (gameManager != null && gameManager.currentDifficulty != lastDifficulty)
        {
            lastDifficulty = gameManager.currentDifficulty;
            PlayMusicForDifficulty(lastDifficulty);
        }
    }

    // Chooses music clip based on difficulty
    public void PlayMusicForDifficulty(DifficultyLevel difficulty)
    {
        switch (difficulty)
        {
            case DifficultyLevel.Easy:
                musicSource.clip = musicEasy;
                break;
            case DifficultyLevel.Medium:
                musicSource.clip = musicMedium;
                break;
            case DifficultyLevel.Hard:
                musicSource.clip = musicHard;
                break;
            case DifficultyLevel.Boss:
                musicSource.clip = musicBoss;
                break;
            default:
                return;
        }

        musicSource.loop = true;
        musicSource.Play();
    }
}
