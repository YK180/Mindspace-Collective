using UnityEngine;

public class BoundaryTeleport : MonoBehaviour
{
    public Transform spawnPoint; // The safe spot to return to

    private void OnTriggerExit(Collider other)
    {
        // Check if the object leaving the boundary is the VR Player
        if (other.CompareTag("Player") || other.GetComponent<CharacterController>())
        {
            // Teleport the player back to the spawn point
            other.transform.position = spawnPoint.position;
            
            Debug.Log("Player wandered too far! Resetting position.");
        }
    }
}