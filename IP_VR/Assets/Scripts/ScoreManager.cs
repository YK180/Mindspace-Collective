using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;
    public static string UserEmail = "Guest@email.com"; // Set during Login

    [Header("Scoring")]
    private int customersServed = 0;
    private float totalRevenue = 0f;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void AddOrder(ToastOrder order)
    {
        if (order == null) return;
        customersServed++;
        totalRevenue += order.price;
        // Update your in-game revenue UI here
    }

    public void SaveScoreAtEndDay()
    {
        string displayName = UserEmail.Split('@')[0]; 
        List<LeaderboardEntry> scores = LoadLeaderboard();
        
        scores.Add(new LeaderboardEntry { name = displayName, score = customersServed });
        
        // Automatic Ranking: Sort by score (highest first)
        scores.Sort((a, b) => b.score.CompareTo(a.score));
        
        // Keep top 10 for a scrollable or larger list
        if (scores.Count > 10) scores.RemoveRange(10, scores.Count - 10);

        SaveLeaderboard(scores);
    }

    private void SaveLeaderboard(List<LeaderboardEntry> scores)
    {
        for (int i = 0; i < scores.Count; i++)
        {
            PlayerPrefs.SetString($"LeaderName_{i}", scores[i].name);
            PlayerPrefs.SetInt($"LeaderScore_{i}", scores[i].score);
        }
        PlayerPrefs.SetInt("LeaderCount", scores.Count);
        PlayerPrefs.Save();
    }

    public List<LeaderboardEntry> LoadLeaderboard()
    {
        List<LeaderboardEntry> scores = new List<LeaderboardEntry>();
        int count = PlayerPrefs.GetInt("LeaderCount", 0);
        for (int i = 0; i < count; i++)
        {
            scores.Add(new LeaderboardEntry {
                name = PlayerPrefs.GetString($"LeaderName_{i}"),
                score = PlayerPrefs.GetInt($"LeaderScore_{i}")
            });
        }
        return scores;
    }

    [System.Serializable]
    public class LeaderboardEntry {
        public string name;
        public int score;
    }


}