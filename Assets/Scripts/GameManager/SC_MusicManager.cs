using UnityEngine;

public class SC_MusicManager : MonoBehaviour
{
    public AudioSource musicSource;

    public AudioClip musicEasy;
    public AudioClip musicMedium;
    public AudioClip musicHard;
    public AudioClip musicBoss;

    private DifficultyLevel lastDifficulty;

    // Called at game start
    void Start()
    {
        lastDifficulty = DifficultyLevel.Easy;
        PlayMusicForDifficulty(lastDifficulty);
    }

    // Called externally when difficulty changes
    public void UpdateMusic(DifficultyLevel currentDifficulty)
    {
        if (currentDifficulty == lastDifficulty)
            return;

        lastDifficulty = currentDifficulty;
        PlayMusicForDifficulty(currentDifficulty);
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
