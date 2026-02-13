using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    [Header("Scoring")]
    private int customersServed = 0;
    private float totalRevenue = 0f;

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
        
        Debug.Log($"Order completed: {order.orderName} (+${order.price:F2}) | Total customers: {customersServed}, Total revenue: ${totalRevenue:F2}");
        
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

    // Reset for new day
    public void ResetDay()
    {
        customersServed = 0;
        totalRevenue = 0f;
        UpdateRevenueDisplay();
    }
}