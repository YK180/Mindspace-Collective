using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class AnxietyManager : MonoBehaviour
{
    [Header("UI Reference")]
    public Slider anxietyBar; 
    public float currentAnxiety = 0;
    public float increaseRate = 10f; 

    [Header("Symptoms")]
    public OverlappingThoughts overlappingThoughts;
    public TunnelVision tunnelVision;
    public HeartbeatEffect heartbeatEffect;
    public SoundDistortion soundDistortion;

    private int activeSymptomIndex = -1;

    void Update()
    {
        if (activeSymptomIndex == -1)
        {
            currentAnxiety += increaseRate * Time.deltaTime;
            if (anxietyBar != null) anxietyBar.value = currentAnxiety;

            if (currentAnxiety >= 100)
            {
                TriggerRandomAttack();
            }
        }

        overlappingThoughts.SetIntensity(activeSymptomIndex == 0 ? 100 : 0);
        tunnelVision.SetIntensity(activeSymptomIndex == 1 ? 100 : 0);
        heartbeatEffect.SetIntensity(activeSymptomIndex == 2 ? 100 : 0);
        soundDistortion.SetIntensity(activeSymptomIndex == 3 ? 100 : 0);
    }

    void TriggerRandomAttack()
    {
        activeSymptomIndex = Random.Range(0, 4); 
        Debug.Log("Anxiety Attack Triggered: " + activeSymptomIndex);

        if (activeSymptomIndex == 2)
        {
            if (heartbeatEffect.heartbeatAudio != null) heartbeatEffect.heartbeatAudio.Play();
            if (heartbeatEffect.breathingAudio != null) heartbeatEffect.breathingAudio.Play();
        }
        if (activeSymptomIndex == 3 && soundDistortion.ringingAudio != null) soundDistortion.ringingAudio.Play();
    }

    public void TryCoping(int mechanismIndex)
    {
        bool isCorrectCoping = false;

        // 0: Toy Cat -> UI (0) & Breathing (2)
        if (mechanismIndex == 0 && (activeSymptomIndex == 0 || activeSymptomIndex == 2)) isCorrectCoping = true;
        
        // 1: Waterfall -> Tunnel Vision (1)
        if (mechanismIndex == 1 && activeSymptomIndex == 1) isCorrectCoping = true;

        // 2: Headphones -> Ringing (3)
        if (mechanismIndex == 2 && activeSymptomIndex == 3) isCorrectCoping = true;

        if (isCorrectCoping)
        {
            ResetAnxiety();
        }
    }

    private void ResetAnxiety()
    {
        if (heartbeatEffect.heartbeatAudio != null) heartbeatEffect.heartbeatAudio.Stop();
        if (heartbeatEffect.breathingAudio != null) heartbeatEffect.breathingAudio.Stop();
        if (soundDistortion.ringingAudio != null) soundDistortion.ringingAudio.Stop();

        activeSymptomIndex = -1;
        currentAnxiety = 0;
        
        overlappingThoughts.SetIntensity(0);
        tunnelVision.SetIntensity(0);
        heartbeatEffect.SetIntensity(0);
        soundDistortion.SetIntensity(0);
    }

    // --- CLASSES MUST BE INSIDE THE SAME FILE TO PREVENT CS0246 ERRORS ---

    [System.Serializable]
    public class OverlappingThoughts {
        public CanvasGroup orderUICanvas;
        public void SetIntensity(float level) {
            if (orderUICanvas == null) return;
            orderUICanvas.alpha = (level > 0) ? 0.3f : 1.0f;
        }
    }

    [System.Serializable]
    public class TunnelVision {
        public Volume globalVolume;
        public void SetIntensity(float level) {
            if (globalVolume != null && globalVolume.profile.TryGet(out Vignette v))
                v.intensity.value = (level / 100f) * 0.6f; 
        }
    }

    [System.Serializable]
    public class HeartbeatEffect {
        public AudioSource heartbeatAudio;
        public AudioSource breathingAudio;
        public void SetIntensity(float level) {
            float v = (level > 0) ? 1.0f : 0f;
            if (heartbeatAudio != null) heartbeatAudio.volume = v;
            if (breathingAudio != null) breathingAudio.volume = v;
        }
    }

    [System.Serializable]
    public class SoundDistortion {
        public AudioSource ringingAudio;
        public void SetIntensity(float level) {
            if (ringingAudio != null) ringingAudio.volume = (level > 0) ? 1.0f : 0f;
        }
    }
}