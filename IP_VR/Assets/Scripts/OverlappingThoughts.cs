using UnityEngine;

public class OverlappingThoughts : MonoBehaviour
{
    public RectTransform[] ghostImages;
    float intensity;

    public void SetIntensity(float anxiety)
    {
        intensity = Mathf.Clamp01(anxiety / 40f);
    }

    void Update()
    {
        for (int i = 0; i < ghostImages.Length; i++)
        {
            Vector2 offset = new Vector2(
                Mathf.Sin(Time.time * 0.5f + i) * 10f * intensity,
                Mathf.Cos(Time.time * 0.4f + i) * 10f * intensity
            );

            ghostImages[i].anchoredPosition = offset;
        }
    }
}
