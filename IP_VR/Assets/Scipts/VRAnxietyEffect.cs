using UnityEngine;

public class VRAnxietyEffect : MonoBehaviour
{
    public AudioSource heartbeatSource;
    public Transform cameraOffset; // Drag your 'Camera Offset' object here
    
    [Range(0, 1)] 
    public float anxietyLevel = 0.5f; // Set to 0.5 to test the effect immediately
    public float shakeAmount = 0.015f; // Keep this small for VR safety

    private Vector3 initialPosition;

    void Start()
    {
        if (cameraOffset != null)
            initialPosition = cameraOffset.localPosition;
    }

    void Update()
    {
        if (anxietyLevel > 0.1f)
        {
            // Handle Heartbeat Audio
            if (!heartbeatSource.isPlaying) heartbeatSource.Play();
            heartbeatSource.volume = anxietyLevel;
            heartbeatSource.pitch = 1f + (anxietyLevel * 0.3f);

            // Handle Subtle VR Shake
            float currentShake = anxietyLevel * shakeAmount;
            cameraOffset.localPosition = initialPosition + Random.insideUnitSphere * currentShake;
        }
        else
        {
            if (heartbeatSource.isPlaying) heartbeatSource.Stop();
            cameraOffset.localPosition = initialPosition;
        }
    }
}