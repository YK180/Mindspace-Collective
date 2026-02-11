using UnityEngine;
using UnityEngine.SceneManagement; // Required for the Quit scene change

public class MenuNavigation : MonoBehaviour
{
    [Header("UI Panels (Same Scene)")]
    public GameObject mainMenusPanel;
    public GameObject settingsPanel;
    public GameObject leaderboardPanel;

    [Header("Scene Transition (New Scene)")]
    public string gameSceneName = "GameScene";
    public string quitTargetSceneName = "LoginScene"; // Name of your Login/Startup scene

    // --- PANEL TOGGLING ---
    
    public void PlayGame()
    {
        Debug.Log("Entering the simulation...");
        // This loads the 3D restaurant scene where the NPCs appear
        SceneManager.LoadScene(gameSceneName);
    }
    
    public void OpenLeaderboard()
    {
        mainMenusPanel.SetActive(false);
        settingsPanel.SetActive(false);
        leaderboardPanel.SetActive(true);
        Debug.Log("Leaderboard Opened");
    }

    public void ReturnToMainMenu()
    {
        leaderboardPanel.SetActive(false);
        settingsPanel.SetActive(false);
        mainMenusPanel.SetActive(true);
        Debug.Log("Returned to Main Menu");
    }

    // --- SCENE CHANGING ---

    public void QuitToLogin()
    {
        Debug.Log("Quitting to " + quitTargetSceneName);
        // This physically stops the current scene and loads the next one
        SceneManager.LoadScene(quitTargetSceneName); 
    }
}