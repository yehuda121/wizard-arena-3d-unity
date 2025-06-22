using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class SC_CheatMenuManager : MonoBehaviour
{
    public GameObject CheatMenuPanel; // Panel to display when P is pressed
    public TMP_Dropdown levelDropdown; // Dropdown for selecting difficulty
    public GameObject inGameMenuPanel; // Reference to the in-game menu panel

    private bool isMenuOpen = false;
    private int originalSelection; // Store the original selected difficulty

    private void Start()
    {
        if (CheatMenuPanel != null)
            CheatMenuPanel.SetActive(false);

        if (levelDropdown != null)
            originalSelection = levelDropdown.value;
    }

    private void Update()
    {
#if UNITY_EDITOR || DEVELOPMENT_BUILD
        if (Input.GetKeyDown(KeyCode.P))
        {
            TryToggleCheatMenu();
        }
#endif
    }

    // Attempt to toggle cheat menu only if in-game menu is not open
    private void TryToggleCheatMenu()
    {
        if (inGameMenuPanel != null && inGameMenuPanel.activeSelf)
        {
            // Don't open cheat menu if in-game menu is currently open
            return;
        }

        ToggleCheatMenu();
    }

    // Called by the "Refill Health" button in the cheat menu
    public void RefillPlayerHealth()
    {
        SC_PlayerHealthSystem playerHealth = FindObjectOfType<SC_PlayerHealthSystem>();
        if (playerHealth != null && !playerHealth.isDead)
        {
            playerHealth.ResetToFull();
        }
    }

    // Toggle the cheat menu and sync with game pause/resume
    private void ToggleCheatMenu()
    {
        isMenuOpen = !isMenuOpen;
        CheatMenuPanel.SetActive(isMenuOpen);

        SC_GameManager gm = FindObjectOfType<SC_GameManager>();
        if (gm != null)
        {
            if (isMenuOpen)
            {
                gm.PauseGame();

                int currentIndex = (int)gm.currentDifficulty;
                levelDropdown.value = currentIndex;
                originalSelection = currentIndex;
            }
            else
            {
                gm.ResumeGame();
            }
        }
    }

    // Called when the user confirms level change
    public void ApplyLevelChange()
    {
        int selected = levelDropdown.value;

        if (selected != originalSelection)
        {
            PlayerPrefs.SetInt("SelectedDifficulty", selected);

            int initialSpawnedEnemies = 0;
            int updatedKillCount = 0;

            switch (selected)
            {
                case 0: initialSpawnedEnemies = 0; updatedKillCount = 0; break;   // Easy
                case 1: initialSpawnedEnemies = 10; updatedKillCount = 10; break; // Medium
                case 2: initialSpawnedEnemies = 20; updatedKillCount = 20; break; // Hard
                case 3: initialSpawnedEnemies = 30; updatedKillCount = 30; break; // Boss
            }

            PlayerPrefs.SetInt("InitialSpawnedEnemies", initialSpawnedEnemies);
            PlayerPrefs.Save();

            // Update kill count of the player so difficulty syncs properly
            PlayerShooting player = FindObjectOfType<PlayerShooting>();
            if (player != null)
            {
                player.stageKillCount = updatedKillCount;
            }

            Time.timeScale = 1f;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        else
        {
            // No change - just close the cheat menu and resume
            ContinueGame();
        }
    }

    // Resume the game and hide cheat menu
    public void ContinueGame()
    {
        SC_GameManager gm = FindObjectOfType<SC_GameManager>();
        if (gm != null)
        {
            gm.ResumeGame();
        }

        if (CheatMenuPanel != null)
            CheatMenuPanel.SetActive(false);

        isMenuOpen = false;
    }

    // Restart game through GameManager
    public void RestartGame()
    {
        SC_GameManager gm = FindObjectOfType<SC_GameManager>();
        if (gm != null)
        {
            gm.RestartGame();
        }
    }
}

