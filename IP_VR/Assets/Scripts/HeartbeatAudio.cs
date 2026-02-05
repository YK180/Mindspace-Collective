using UnityEngine;

public class HeartbeatAudio : MonoBehaviour
{
    public AudioSource source;

    public void SetIntensity(float anxiety)
    {
        source.volume = Mathf.Lerp(0, 0.6f, anxiety / 60f);
    }
}
