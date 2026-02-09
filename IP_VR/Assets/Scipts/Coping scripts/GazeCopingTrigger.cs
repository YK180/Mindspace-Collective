using UnityEngine;

public class GazeCopingTrigger : MonoBehaviour
{
    [Header("References")]
    public AnxietyManager anxietyManager; 
    public Transform playerCamera;   
    public Transform waterfallTarget; 

    [Header("Settings")]
    public float lookThreshold = 0.9f; 
    private bool isPlayerInZone = false;

    void Update()
    {
        // Only check gaze if the player is actually in the zone
        if (isPlayerInZone && anxietyManager != null)
        {
            if (IsPlayerLookingAtWaterfall())
            {
                anxietyManager.TryCoping(1); // 1 is the index for Waterfall
                Debug.Log("Coping Mechanism Triggered: Looked at Waterfall!");
            }
        }

        
    }

    private bool IsPlayerLookingAtWaterfall()
    {
        if (playerCamera == null || waterfallTarget == null) return false;

        // Calculate direction from camera to waterfall
        Vector3 directionToWaterfall = (waterfallTarget.position - playerCamera.position).normalized;
        // Compare camera forward direction to waterfall direction
        float lookPercentage = Vector3.Dot(playerCamera.forward, directionToWaterfall);

        return lookPercentage > lookThreshold;
    }

    // THE FIX: Better detection for the VR player
    private void OnTriggerEnter(Collider other)
    {
        // Check if the object entering is the camera or has a camera component
        if (other.CompareTag("MainCamera") || other.GetComponentInChildren<Camera>() != null)
        {
            isPlayerInZone = true;
            Debug.Log("Player entered waterfall zone");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("MainCamera") || other.GetComponentInChildren<Camera>() != null)
        {
            isPlayerInZone = false;
            Debug.Log("Player left waterfall zone");
        }
    }
}