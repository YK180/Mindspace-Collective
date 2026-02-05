using UnityEngine;

public class HeartbeatEffect : MonoBehaviour
{
    Vector3 startPos;
    float intensity;

    void Start()
    {
        startPos = transform.localPosition;
    }

    public void SetIntensity(float anxiety)
    {
        intensity = Mathf.Clamp01(anxiety / 70f);
    }

    void Update()
    {
        float shake = Mathf.Sin(Time.time * 2f) * 0.01f * intensity;
        transform.localPosition = startPos + Vector3.up * shake;
    }
}
