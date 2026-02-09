using UnityEngine;
using UnityEngine.Rendering.Universal; // Needs URP for Vignette

public class AnxietyManager : MonoBehaviour
{
    [Range(0,100)]
    public float anxietyLevel = 0;

    public OverlappingThoughts overlappingThoughts;
    public TunnelVision tunnelVision;
    public HeartbeatEffect heartbeatEffect;
    public SoundDistortion soundDistortion;

    [System.Serializable]
    public class OverlappingThoughts
    {
        public CanvasGroup orderUICanvas; // Assign your order UI panel here
        
        public void SetIntensity(float level)
        {
            if (orderUICanvas == null) return;

            // As anxiety goes up, the UI starts to flicker and fade
            // creating the "forgetting" effect
            float alpha = 1.0f - (level / 150f); 
            orderUICanvas.alpha = Mathf.Clamp(alpha, 0.3f, 1.0f);
        }
    }

    [System.Serializable]
    public class TunnelVision
    {
        public UnityEngine.Rendering.Volume globalVolume;
        private Vignette vignette;

        public void SetIntensity(float level)
        {
            if (globalVolume == null) return;
            
            if (globalVolume.profile.TryGet(out vignette))
            {
                // Vignette gets stronger (closer to center) as anxiety rises
                vignette.intensity.value = (level / 100f) * 0.6f; 
            }
        }
    }

    [System.Serializable]
    public class HeartbeatEffect
    {
        public AudioSource heartbeatAudio;
        public Transform cameraTransform; // Assign Main Camera (XR Origin)

        public void SetIntensity(float level)
        {
            if (heartbeatAudio != null)
            {
                // Heartbeat gets louder and faster
                heartbeatAudio.volume = level / 100f;
                heartbeatAudio.pitch = 1f + (level / 200f);
            }

            if (cameraTransform != null && level > 50)
            {
                // Screen starts shaking slightly at high anxiety
                float shakeAmount = (level - 50) * 0.001f;
                cameraTransform.localPosition += Random.insideUnitSphere * shakeAmount;
            }
        }
    }

    [System.Serializable]
    public class SoundDistortion
    {
        public AudioSource ringingAudio;
        public AudioLowPassFilter ambientFilter;

        public void SetIntensity(float level)
        {
            if (ringingAudio != null)
            {
                // High pitch ringing fades in
                ringingAudio.volume = level / 100f;
            }

            if (ambientFilter != null)
            {
                // Ambient sounds become "muffled" (Low pass gets lower)
                ambientFilter.cutoffFrequency = 5000f - (level * 40f);
            }
        }
    }

    void Update()
    {
        //overlappingThoughts.SetIntensity(anxietyLevel);
        //tunnelVision.SetIntensity(anxietyLevel);
        //heartbeatEffect.SetIntensity(anxietyLevel);
        soundDistortion.SetIntensity(anxietyLevel);
    }

    
}
