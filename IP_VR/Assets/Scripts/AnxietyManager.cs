using UnityEngine;

public class AnxietyManager : MonoBehaviour
{
    [Range(0,100)]
    public float anxietyLevel = 0;

    public OverlappingThoughts overlappingThoughts;
    public TunnelVision tunnelVision;
    public HeartbeatEffect heartbeatEffect;
    public SoundDistortion soundDistortion;

    void Update()
    {
        overlappingThoughts.SetIntensity(anxietyLevel);
        tunnelVision.SetIntensity(anxietyLevel);
        heartbeatEffect.SetIntensity(anxietyLevel);
        soundDistortion.SetIntensity(anxietyLevel);
    }
}
