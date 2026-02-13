using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    [Header("Scoring")]
    private int customersServed = 0;
    private float totalRevenue = 0f;
    private Dictionary<string, int> toastOrderCounts = new Dictionary<string, int>(); // Track each toast type

    [Header("UI References")]
    public TextMeshProUGUI revenueText; // Display for current revenue

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        UpdateRevenueDisplay();
    }

    // Updated to accept the specific order and use its price
    public void AddOrder(ToastOrder order)
    {
        if (order == null)
        {
            Debug.LogWarning("Trying to add null order!");
            return;
        }

        customersServed++;
        totalRevenue += order.price;
        
        // Track this specific toast type
        if (toastOrderCounts.ContainsKey(order.orderName))
        {
            toastOrderCounts[order.orderName]++;
        }
        else
        {
            toastOrderCounts[order.orderName] = 1;
        }
        
        Debug.Log($"Order completed: {order.orderName} (+${order.price:F2}) | Total customers: {customersServed}, Total revenue: ${totalRevenue:F2}");
        Debug.Log($"{order.orderName} count: {toastOrderCounts[order.orderName]}");
        
        UpdateRevenueDisplay();
    }

    void UpdateRevenueDisplay()
    {
        if (revenueText != null)
        {
            revenueText.text = $"${totalRevenue:F2}";
        }
    }

    // Getters for end of day stats
    public int GetCustomersServed()
    {
        return customersServed;
    }

    public float GetTotalRevenue()
    {
        return totalRevenue;
    }
    
    // Get count of a specific toast type
    public int GetToastCount(string toastName)
    {
        if (toastOrderCounts.ContainsKey(toastName))
        {
            return toastOrderCounts[toastName];
        }
        return 0;
    }
    
    // Get all toast counts (useful for end of day summary if you want it later)
    public Dictionary<string, int> GetAllToastCounts()
    {
        return new Dictionary<string, int>(toastOrderCounts);
    }
    
    // Print breakdown to console
    public void PrintOrderBreakdown()
    {
        Debug.Log("=== ORDER BREAKDOWN ===");
        foreach (KeyValuePair<string, int> entry in toastOrderCounts)
        {
            Debug.Log($"{entry.Key}: {entry.Value} orders");
        }
        Debug.Log($"Total Customers: {customersServed}");
        Debug.Log($"Total Revenue: ${totalRevenue:F2}");
        Debug.Log("======================");
    }

    // Reset for new day
    public void ResetDay()
    {
        customersServed = 0;
        totalRevenue = 0f;
        toastOrderCounts.Clear();
        UpdateRevenueDisplay();
    }
}