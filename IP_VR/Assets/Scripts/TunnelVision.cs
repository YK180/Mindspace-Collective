using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class TunnelVision : MonoBehaviour
{
    public Volume volume;
    Vignette vignette;

    void Start()
    {
        volume.profile.TryGet(out vignette);
    }

    public void SetIntensity(float anxiety)
    {
        vignette.intensity.value = Mathf.Lerp(0, 0.45f, anxiety / 60f);
    }
}
