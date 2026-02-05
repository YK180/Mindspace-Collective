using UnityEngine;

public class SauceBowl : MonoBehaviour
{
    public SauceType sauceType;
    public Material sauceMaterial;

    private void OnTriggerEnter(Collider other)
    {
        LadleSauce ladle = other.GetComponentInParent<LadleSauce>();

        if (ladle != null && !ladle.hasSauce)
        {
            ladle.FillSauce(sauceType, sauceMaterial);
        }
    }
}
