using UnityEngine;
using UnityEngine.UI;

public class ShowOrderUIOnClick : MonoBehaviour
{
    [Header("Customer reference (assign in Inspector)")]
    public CustomerAI customer; // The customer whose order will be displayed

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
            Debug.LogError("No Button component found on this GameObject!");
        }
    }

    void OnButtonClicked()
{
    Debug.Log("Button clicked!");
    
    if (OrderUIManager.Instance == null)
    {
        Debug.LogError("OrderUIManager.Instance is null!");
        return;
    }

    if (customer == null)
    {
        Debug.LogError("Customer reference is null!");
        return;
    }

    ToastOrder order = customer.GetCurrentOrder();
    if (order == null)
    {
        Debug.LogError("Customer has no order!");
        return;
    }

    OrderUIManager.Instance.DisplayOrder(order);
    Debug.Log($"Order displayed: {order.orderName}");
}

}
