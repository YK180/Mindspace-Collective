using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OrderUIManager : MonoBehaviour
{
    public static OrderUIManager Instance;

    [Header("UI References")]
    public GameObject orderPanel; // Panel in top-right corner
    public TextMeshProUGUI orderNameText;
    public TextMeshProUGUI orderDescriptionText;
    public Image orderIconImage; // Optional: shows toast icon
    
    private ToastOrder currentDisplayedOrder;

    void Awake()
    {
        // Singleton pattern
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        
        // Hide order panel at start
        if (orderPanel != null)
        {
            orderPanel.SetActive(false);
        }
    }

    public void DisplayOrder(ToastOrder order)
    {
        if (order == null) return;
        
        currentDisplayedOrder = order;
        
        // Show the panel
        if (orderPanel != null)
        {
            orderPanel.SetActive(true);
        }
        
        // Update text
        if (orderNameText != null)
        {
            orderNameText.text = order.orderName;
        }
        
        if (orderDescriptionText != null)
        {
            orderDescriptionText.text = order.description;
        }
        
        // Update icon if available
        if (orderIconImage != null && order.orderIcon != null)
        {
            orderIconImage.sprite = order.orderIcon;
            orderIconImage.enabled = true;
        }
        else if (orderIconImage != null)
        {
            orderIconImage.enabled = false;
        }
    }

    public void ClearOrder()
    {
        currentDisplayedOrder = null;
        
        if (orderPanel != null)
        {
            orderPanel.SetActive(false);
        }
    }

    public ToastOrder GetCurrentOrder()
    {
        return currentDisplayedOrder;
    }
}