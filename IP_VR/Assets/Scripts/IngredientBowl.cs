using UnityEngine;

public class IngredientBowl : MonoBehaviour
{
    [Header("Ingredient Settings")]
    public GameObject ingredientPrefab;   // Banana slice prefab
    public Transform spawnPoint;           // Optional spawn point (can be empty)

    [Header("Debug")]
    public bool enableDebug = true;

    private void OnTriggerEnter(Collider other)
    {
        if (enableDebug)
        {
            Debug.Log(
                "[IngredientBowl] Trigger entered by: " +
                other.name +
                " | Tag: " +
                other.tag
            );
        }

        // Only react to hands
        if (!other.CompareTag("Hand"))
            return;

        SpawnIngredient();
    }

    void SpawnIngredient()
    {
        if (ingredientPrefab == null)
        {
            Debug.LogError("[IngredientBowl] ❌ Ingredient Prefab is NOT assigned!");
            return;
        }

        Vector3 spawnPos =
            spawnPoint != null
            ? spawnPoint.position
            : transform.position + Vector3.up * 0.1f;

        GameObject ingredient = Instantiate(
            ingredientPrefab,
            spawnPos,
            Quaternion.identity
        );

        if (enableDebug)
        {
            Debug.Log("[IngredientBowl] ✅ Spawned ingredient: " + ingredient.name);
        }
    }
}
