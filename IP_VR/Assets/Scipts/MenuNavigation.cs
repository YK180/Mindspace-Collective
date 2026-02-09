using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuNavigation : MonoBehaviour

{
    [Header("UI Panels")]
    public GameObject mainMenusPanel;    // The main screen
    public GameObject settingsPanel;     // The Settings screen
    public GameObject leaderboardPanel; // The NEW Leaderboard panel

    [Header("Scene Transition (New Scene)")]
    public string quitTargetSceneName = "Raphael_login";


    // Function to open the Leaderboard
    public void OpenLeaderboard()
    {
        // Hide other panels and show the leaderboard

        if (mainMenusPanel != null) mainMenusPanel.SetActive(false);

        if (settingsPanel != null) settingsPanel.SetActive(false);

        if (leaderboardPanel != null) leaderboardPanel.SetActive(true);

        Debug.Log("Leaderboard Panel Opened");
    }



    // Function to go back to Main Menu from any panel
    public void ReturnToMainMenu()
    {

        if (leaderboardPanel != null) leaderboardPanel.SetActive(false);

        if (settingsPanel != null) settingsPanel.SetActive(false);

        if (mainMenusPanel != null) mainMenusPanel.SetActive(true);

        

        Debug.Log("Returned to Main Menu");
    }

    // Existing Quit logic
    public void QuitToLogin()
    {

        Debug.Log("Quitting to " + quitTargetSceneName);

        // This physically stops the current scene and loads the next one

        SceneManager.LoadScene(quitTargetSceneName); 
    }

}





