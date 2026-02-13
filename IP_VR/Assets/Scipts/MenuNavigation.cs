using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections.Generic;

public class MenuNavigation : MonoBehaviour
{
    [Header("UI Panels")]
    public GameObject mainMenusPanel;
    public GameObject settingsPanel;
    public GameObject leaderboardPanel;

    [Header("Leaderboard Columns")]
    public TextMeshProUGUI rankColumn;
    public TextMeshProUGUI userColumn;
    public TextMeshProUGUI scoreColumn;

    [Header("Scene Transition")]
    public string gameSceneName = "Raphael_game";
    public string quitTargetSceneName = "Raphael_login"; 

    public void PlayGame()
    {
        SceneManager.LoadScene(gameSceneName);
    }

    public void OpenLeaderboard()
    {
        mainMenusPanel.SetActive(false);
        settingsPanel.SetActive(false);
        leaderboardPanel.SetActive(true);

        // Fetch and display sorted scores
        DisplayLeaderboardData();
    }

    private void DisplayLeaderboardData()
    {
        rankColumn.text = "";
        userColumn.text = "";
        scoreColumn.text = "";

        ScoreManager sm = FindFirstObjectByType<ScoreManager>();
        List<ScoreManager.LeaderboardEntry> scores = (sm != null) ? sm.LoadLeaderboard() : new List<ScoreManager.LeaderboardEntry>();

        for (int i = 0; i < scores.Count; i++)
        {
            rankColumn.text += (i + 1) + ".\n";
            userColumn.text += scores[i].name + "\n";
            scoreColumn.text += scores[i].score + "\n";
        }
    }

    public void OpenSettings()
    {
        mainMenusPanel.SetActive(false);
        leaderboardPanel.SetActive(false);
        settingsPanel.SetActive(true);
    }

    public void ReturnToMainMenu()
    {
        leaderboardPanel.SetActive(false);
        settingsPanel.SetActive(false);
        mainMenusPanel.SetActive(true);
    }

    public void QuitToLogin()
    {
        SceneManager.LoadScene(quitTargetSceneName); 
    }
}