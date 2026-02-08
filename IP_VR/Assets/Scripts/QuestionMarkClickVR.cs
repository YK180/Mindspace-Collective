using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class QuestionMarkClickVR : MonoBehaviour
{
    public CustomerAI customer;
    private XRSimpleInteractable interactable;

    void Awake()
    {
        // Add XR Simple Interactable if not already present
        interactable = GetComponent<XRSimpleInteractable>();
        if (interactable == null)
        {
            interactable = gameObject.AddComponent<XRSimpleInteractable>();
        }
        
        // Subscribe to select events (when VR controller clicks/pokes)
        interactable.selectEntered.AddListener(OnSelect);
    }

    void OnDestroy()
    {
        // Clean up listener
        if (interactable != null)
        {
            interactable.selectEntered.RemoveListener(OnSelect);
        }
    }

    void OnSelect(SelectEnterEventArgs args)
    {
        if (customer != null)
        {
            customer.OnQuestionMarkClicked();
        }
    }
}