using UnityEngine;

public class DistanceReset : MonoBehaviour
{
    [Header("Settings")]
    public Transform centerPoint;    // Drag your counter/computer here
    public float maxDistance = 3.0f; // Distance in meters before reset
    
    // We hardcode the reset position to the origin (0,0,0)
    private Vector3 resetPosition = new Vector3(0, 0, 0);

    void Update()
    {
        // Calculate how far the player has moved from the centerPoint
        float distance = Vector3.Distance(transform.position, centerPoint.position);

        // If they exceed the max distance, snap them back to the origin
        if (distance > maxDistance)
        {
            TeleportBack();
        }
    }

    void TeleportBack()
    {
        Debug.Log("Player wandering! Resetting to origin.");
        
        // Move the player back to 0,0,0
        transform.position = resetPosition;
        
        // Optional: Reset velocity if using a Rigidbody/Character Controller
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null) rb.linearVelocity = Vector3.zero;
    }
}