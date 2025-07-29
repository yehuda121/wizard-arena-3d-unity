using UnityEngine;

public class SC_InGameMenu : MonoBehaviour
{
    public GameObject panel; // The in-game menu panel

    void Start()
    {
        if (panel != null)
            panel.SetActive(false); // Hide the menu at the beginning
    }

    // Called when the menu button is opened
    public void OpenMenu()
    {
        if (panel != null)
        {
            panel.SetActive(true);  // Show the menu panel
            Time.timeScale = 0f;    // Pause the game
        }
    }

    // Called when the user clicks "Continue"
    public void ContinueGame()
    {
        if (panel != null)
            panel.SetActive(false); // Hide the menu panel

        Time.timeScale = 1f; // Resume the game
    }

    // Called when the user clicks "Restart Game"
    public void RestartGame()
    {
        // Find the GameManager and call its RestartGame method
        SC_GameManager gm = FindObjectOfType<SC_GameManager>();
        if (gm != null)
        {
            gm.RestartGame(); // GameManager will handle full reset and scene reload
        }
    }
}
