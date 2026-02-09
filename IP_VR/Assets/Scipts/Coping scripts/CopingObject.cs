using UnityEngine;

public class CopingObject : MonoBehaviour
{
    [Header("References")]
    public AnxietyManager anxietyManager; 
    
    [Tooltip("0 for Toy Cat, 2 for Headphones")]
    public int copingIndex = 0; 

    public void OnGrabCoping()
    {
        if (anxietyManager != null)
        {
            // Fix: Changed to TryCoping
            anxietyManager.TryCoping(copingIndex);
            Debug.Log("Coping via Grab attempted.");
        }
    }

    public void OnTouchCoping()
    {
        if (anxietyManager != null)
        {
            // Fix: Changed to TryCoping
            anxietyManager.TryCoping(copingIndex);
            Debug.Log("Coping via Touch attempted.");
        }
    }
}