using UnityEngine;

public class HeadphoneCoping : MonoBehaviour
{
    public AnxietyManager anxietyManager; 

    public void OnGrabHeadphones()
    {
        if (anxietyManager != null)
        {
            // Changed to TryCoping(2) for Headphones
            anxietyManager.TryCoping(2);
        }
    }
}