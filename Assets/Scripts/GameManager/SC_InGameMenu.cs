using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class SC_InGameMenu : MonoBehaviour
{
    public GameObject panel; // InGameMenuPanel
    public TMP_Dropdown levelDropdown;

    private int originalSelection;

    void Start()
    {
        if (panel != null)
            panel.SetActive(false);

        if (levelDropdown != null)
            originalSelection = levelDropdown.value;
    }

    public void OpenMenu()
    {
        if (panel != null)
        {
            panel.SetActive(true);
            Time.timeScale = 0f;
        }

        SC_GameManager gm = FindObjectOfType<SC_GameManager>();
        if (gm != null)
        {
            int currentIndex = (int)gm.currentDifficulty;
            levelDropdown.value = currentIndex;
            originalSelection = currentIndex;
        }
    }


    public void ContinueGame()
    {
        if (panel != null)
            panel.SetActive(false);

        Time.timeScale = 1f;
    }

    public void RestartOrApply()
    {
        int selected = levelDropdown.value;

        if (selected != originalSelection)
        {
            PlayerPrefs.SetInt("SelectedDifficulty", selected);

            int initialSpawnedEnemies = 0;
            switch (selected)
            {
                case 0: initialSpawnedEnemies = 0; break;  // Easy
                case 1: initialSpawnedEnemies = 10; break; // Medium
                case 2: initialSpawnedEnemies = 20; break; // Hard
                case 3: initialSpawnedEnemies = 30; break; // Boss
            }

            PlayerPrefs.SetInt("InitialSpawnedEnemies", initialSpawnedEnemies);
            PlayerPrefs.Save();

            Time.timeScale = 1f;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        else
        {
            ContinueGame();
        }
    }


}
