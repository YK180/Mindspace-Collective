using UnityEngine;
using UnityEngine.UI;

public class OrderUI : MonoBehaviour
{
    Image orderImage;

    void Awake()
    {
        orderImage = GetComponent<Image>();
        orderImage.enabled = false; // hidden by default
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
