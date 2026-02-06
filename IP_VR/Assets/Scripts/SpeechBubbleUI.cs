using UnityEngine;
using UnityEngine.UI;

public class SpeechBubbleUI : MonoBehaviour
{
    public Image bubbleImage;
    public Sprite questionMarkSprite;

    private CustomerAI customer;
    private bool revealed = false;

    void Start()
    {
        customer = GetComponentInParent<CustomerAI>();
        bubbleImage.sprite = questionMarkSprite;
    }

    public void OnBubbleClicked()
    {
        if (revealed) return;

        bubbleImage.sprite = customer.chosenOrder.orderSprite;
        revealed = true;

        OrderUIManager.Instance.ShowOrder(customer.chosenOrder);
    }
}
