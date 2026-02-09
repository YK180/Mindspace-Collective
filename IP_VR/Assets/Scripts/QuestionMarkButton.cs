using UnityEngine;
using UnityEngine.UI;

public class QuestionMarkButton : MonoBehaviour
{
    [Header("Target Customer")]
    public CustomerAI targetCustomer; // Assign the customer this button belongs to

    [Header("Offset Above Customer")]
    public Vector3 screenOffset = new Vector3(0, 50, 0); // Pixels above the customer on screen

    private Button button;

    void Awake()
    {
        button = GetComponent<Button>();
        if (button != null)
        {
            button.onClick.AddListener(OnButtonClicked);
        }
        else
        {
            Debug.LogError("No Button component found on QuestionMarkButton prefab!");
        }
    }

    void Update()
    {
        if (targetCustomer != null)
        {
            // Convert customer's world position to screen position and add offset
            Vector3 screenPos = Camera.main.WorldToScreenPoint(targetCustomer.transform.position);
            transform.position = screenPos + screenOffset;
        }
    }

    void OnButtonClicked()
    {
        if (targetCustomer != null)
        {
            ToastOrder order = targetCustomer.GetCurrentOrder();
            if (OrderUIManager.Instance != null && order != null)
            {
                // Show order UI
                OrderUIManager.Instance.DisplayOrder(order);
                Debug.Log($"Order displayed: {order.orderName}");
                
                // Optionally destroy/hide the question mark button
                Destroy(gameObject);
            }
        }
    }
}
