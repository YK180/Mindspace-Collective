using UnityEngine;
using UnityEditor;

public class ToastOrderGenerator : MonoBehaviour
{
    [MenuItem("Food Truck/Generate All Toast Orders")]
    public static void GenerateToastOrders()
    {
        // Define all toast types
        string[] toastNames = new string[]
        {
            "Toast",
            "Sugar Butter",
            "Sugar Butter with Honey and Butter",
            "Sugar Butter with Cream Cheese",
            "Matcha",
            "Chocolate",
            "Peanut Butter",
            "Honey",
            "Chocolate and Banana",
            "Strawberry Jam with Strawberries",
            "Cream Cheese and Blueberries",
            "Cream Cheese and Strawberries"
        };

        // Create folder if it doesn't exist
        string folderPath = "Assets/ToastOrders";
        if (!AssetDatabase.IsValidFolder(folderPath))
        {
            AssetDatabase.CreateFolder("Assets", "ToastOrders");
        }

        // Generate each toast order
        foreach (string toastName in toastNames)
        {
            ToastOrder order = ScriptableObject.CreateInstance<ToastOrder>();
            order.orderName = toastName;
            order.description = toastName; // You can customize descriptions later
            
            // Create the asset
            string fileName = toastName.Replace(" ", "_").Replace("and", "&");
            string assetPath = $"{folderPath}/{fileName}.asset";
            
            AssetDatabase.CreateAsset(order, assetPath);
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        
        Debug.Log($"Successfully created {toastNames.Length} toast orders in {folderPath}!");
        Debug.Log("Now assign the sprite icons to each toast order in the Inspector.");
    }
}