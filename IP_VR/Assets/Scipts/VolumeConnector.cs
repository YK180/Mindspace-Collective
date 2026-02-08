using UnityEngine;
using UnityEngine.UI;

public class VolumeConnector : MonoBehaviour
{
    void Start()
    {
        Slider slider = GetComponent<Slider>();
        
        // Set the slider's position to match the saved volume
        slider.value = PlayerPrefs.GetFloat("MusicVolume", 0.5f);

        // Tell the slider to update the Global Manager when moved
        slider.onValueChanged.AddListener(val => {
            if (GlobalAudioManager.instance != null)
                GlobalAudioManager.instance.ChangeVolume(val);
        });
    }
}