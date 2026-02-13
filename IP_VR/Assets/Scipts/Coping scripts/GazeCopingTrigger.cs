using UnityEngine;

public class LandCopingTrigger : MonoBehaviour
{
    [Header("References")]
    public AnxietyManager anxietyManager; 

    [Header("Settings")]
    public string playerTag = "MainCamera"; // Detects the player's head

    private void OnTriggerEnter(Collider other)
    {
        // Check if the object entering the box is the player's camera or has a Camera component
        if (other.CompareTag(playerTag) || other.GetComponentInChildren<Camera>() != null)
        {
            if (anxietyManager != null)
            {
                // Specifically trigger the coping mechanism for Tunnel Vision (Index 1)
                anxietyManager.TryCoping(1);
                Debug.Log("Success: Player entered land zone. Tunnel Vision cured!");
            }
        }
    }
}