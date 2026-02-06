using UnityEngine;
using UnityEngine.UI;

public class OrderUIManager : MonoBehaviour
{
    public static OrderUIManager Instance;

    public Image orderImage;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void ShowOrder(OrderData order)
    {
        orderImage.sprite = order.orderSprite;
        orderImage.enabled = true;
    }

    public void ClearOrder()
    {
        orderImage.enabled = false;
    }
}
