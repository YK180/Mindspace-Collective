using UnityEngine;

public class OrderCollectionPoint : MonoBehaviour
{
    [Header("Visual Feedback")]
    public Material highlightMaterial; // Optional: material when order is placed here
    private Material originalMaterial;
    private Renderer rend;
    
    [Header("Debug")]
    public bool showDebugMessages = true;

    void Start()
    {
        rend = GetComponent<Renderer>();
        if (rend != null)
        {
            originalMaterial = rend.material;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // Check if it's a toast/food item being placed
        if (other.CompareTag("Food") || other.name.Contains("Toast") || other.name.Contains("Bread"))
        {
            if (showDebugMessages)
                Debug.Log($"Food item placed on collection point: {other.name}");
            
            OnOrderDelivered(other.gameObject);
        }
    }

    void OnOrderDelivered(GameObject foodItem)
    {
        // Visual feedback - highlight the collection point
        if (rend != null && highlightMaterial != null)
        {
            rend.material = highlightMaterial;
            Invoke("ResetMaterial", 0.5f);
        }
        
        // Get the current order from OrderUIManager
        if (OrderUIManager.Instance != null)
        {
            ToastOrder currentOrder = OrderUIManager.Instance.GetCurrentOrder();
            
            if (currentOrder != null)
            {
                // For now, assume any food item completes the order
                // Later you can add ingredient checking here
                
                if (showDebugMessages)
                    Debug.Log($"Order completed: {currentOrder.orderName}");
                
                CompleteOrder(foodItem);
            }
            else
            {
                if (showDebugMessages)
                    Debug.LogWarning("No active order to complete!");
            }
        }
    }

    void CompleteOrder(GameObject foodItem)
    {
        // Find the current customer (the one at the order point)
        CustomerAI currentCustomer = FindCurrentCustomer();
        
        if (currentCustomer != null)
        {
            if (showDebugMessages)
                Debug.Log($"Customer {currentCustomer.name} received their order!");
            
            // Customer leaves
            currentCustomer.LeaveQueue();
            
            // Clear the order UI
            if (OrderUIManager.Instance != null)
            {
                OrderUIManager.Instance.ClearOrder();
            }
            
            // Destroy the food item
            Destroy(foodItem);
        }
        else
        {
            if (showDebugMessages)
                Debug.LogWarning("No customer found to receive the order!");
        }
    }

    CustomerAI FindCurrentCustomer()
    {
        // Find all customers in the scene
        CustomerAI[] allCustomers = FindObjectsOfType<CustomerAI>();
        
        // Return the customer at the order point
        foreach (CustomerAI customer in allCustomers)
        {
            if (CustomerQueueManager.Instance != null)
            {
                if (CustomerQueueManager.Instance.IsCustomerAtOrderPoint(customer))
                {
                    return customer;
                }
            }
            else
            {
                // If no queue manager, return first customer found
                return customer;
            }
        }
        
        return null;
    }

    void ResetMaterial()
    {
        if (rend != null && originalMaterial != null)
        {
            rend.material = originalMaterial;
        }
    }
}