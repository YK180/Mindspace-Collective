using UnityEngine;

[System.Serializable]
public class SaucePrefabEntry
{
    public SauceType sauceType;
    public GameObject saucePrefab;
}

public class SauceReceiver : MonoBehaviour
{
    [Header("Sockets on the bread")]
    public Transform[] sauceSockets;

    [Header("Sauce prefabs mapping")]
    public SaucePrefabEntry[] saucePrefabs;

    private int currentSocket = 0;

    private void OnTriggerEnter(Collider other)
    {
        // Ladle collider might be a child
        LadleSauce ladle = other.GetComponentInParent<LadleSauce>();

        if (ladle == null)
            return;

        if (!ladle.hasSauce)
            return;

        if (currentSocket >= sauceSockets.Length)
            return;

        GameObject prefabToSpawn = GetSaucePrefab(ladle.currentSauce);

        if (prefabToSpawn == null)
        {
            Debug.LogWarning("No sauce prefab found for " + ladle.currentSauce);
            return;
        }

        // Spawn as child of socket (NO physics)
        GameObject sauce = Instantiate(
            prefabToSpawn,
            sauceSockets[currentSocket]
        );

        // Force clean local transform
        sauce.transform.localPosition = Vector3.zero;
        sauce.transform.localRotation = Quaternion.identity;
        sauce.transform.localScale = Vector3.one;

        Physics.SyncTransforms();

        ladle.EmptySauce();
        currentSocket++;
    }

    private GameObject GetSaucePrefab(SauceType type)
    {
        foreach (SaucePrefabEntry entry in saucePrefabs)
        {
            if (entry.sauceType == type)
            {
                return entry.saucePrefab;
            }
        }

        return null;
    }
}
