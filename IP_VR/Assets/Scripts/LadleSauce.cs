using UnityEngine;

public class LadleSauce : MonoBehaviour
{
    public bool hasSauce = false;
    public SauceType currentSauce;

    public GameObject sauceVisual;

    void Start()
    {
        sauceVisual.SetActive(false);
    }

    public void FillSauce(SauceType sauceType, Material sauceMat)
    {
        hasSauce = true;
        currentSauce = sauceType;

        sauceVisual.SetActive(true);
        sauceVisual.GetComponent<MeshRenderer>().material = sauceMat;

        Debug.Log("Ladle filled with " + sauceType);
    }

    public void EmptySauce()
    {
        hasSauce = false;
        sauceVisual.SetActive(false);

        Debug.Log("Ladle emptied");
    }
}
