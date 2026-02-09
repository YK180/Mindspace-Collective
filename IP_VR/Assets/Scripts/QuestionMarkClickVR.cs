using UnityEngine;

public class QuestionMarkClickVR : MonoBehaviour
{
    public CustomerAI customer;
    private bool hasBeenClicked = false;

    // This works with VR controller collisions/triggers
    void OnTriggerEnter(Collider other)
    {
        // Check if a VR controller touched the question mark
        if (!hasBeenClicked && (other.CompareTag("Player") || other.name.Contains("Controller") || other.name.Contains("Interactor")))
        {
            TriggerClick();
        }
    }

    // This can also be called from external scripts if needed
    public void TriggerClick()
    {
        if (!hasBeenClicked && customer != null)
        {
            hasBeenClicked = true;
            customer.OnQuestionMarkClicked();
        }
    }
}