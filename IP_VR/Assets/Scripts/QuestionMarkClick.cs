using UnityEngine;
using UnityEngine.EventSystems;

public class QuestionMarkClick : MonoBehaviour, IPointerClickHandler
{
    public CustomerAI customer;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (customer != null)
        {
            customer.OnQuestionMarkClicked();
        }
    }
}