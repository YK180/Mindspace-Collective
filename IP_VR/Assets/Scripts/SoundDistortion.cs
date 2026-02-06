using UnityEngine;
using UnityEngine.Audio;

public class SoundDistortion : MonoBehaviour
{
    public AudioMixer mixer;

    public void SetIntensity(float anxiety)
    {
        float t = Mathf.Clamp01(anxiety / 50f);

        mixer.SetFloat("BGVolume", Mathf.Lerp(-10f, 0f, t));
        mixer.SetFloat("RingVolume", Mathf.Lerp(-80f, -10f, t));
    }
}
