using UnityEngine;
using UnityEditor;
using System.IO;

public class ToastSpriteAssigner : EditorWindow
{
    [MenuItem("Food Truck/Assign Toast Sprites")]
    public static void ShowWindow()
    {
        GetWindow<ToastSpriteAssigner>("Toast Sprite Assigner");
    }

    void OnGUI()
    {
        GUILayout.Label("Quick Toast Sprite Assignment", EditorStyles.boldLabel);
        GUILayout.Space(10);
        
        GUILayout.Label("Instructions:", EditorStyles.boldLabel);
        GUILayout.Label("1. Make sure your sprites are named exactly like the toast orders");
        GUILayout.Label("   (e.g., 'Plain_Toast.png', 'Cinnamon_Sugar.png')");
        GUILayout.Label("2. Place all sprites in Assets/Sprites/ToastIcons/");
        GUILayout.Label("3. Click the button below to auto-assign them");
        GUILayout.Space(10);
        
        if (GUILayout.Button("Auto-Assign Sprites to Toast Orders", GUILayout.Height(30)))
        {
            AssignSprites();
        }
        
        GUILayout.Space(20);
        GUILayout.Label("Or manually drag and drop sprites in the Inspector:", EditorStyles.wordWrappedLabel);
    }

    void AssignSprites()
    {
        string toastOrderPath = "Assets/ToastOrders";
        string spritePath = "Assets/Sprites/ToastIcons";
        
        // Check if folders exist
        if (!AssetDatabase.IsValidFolder(toastOrderPath))
        {
            EditorUtility.DisplayDialog("Error", "ToastOrders folder not found! Generate toast orders first.", "OK");
            return;
        }
        
        if (!AssetDatabase.IsValidFolder(spritePath))
        {
            EditorUtility.DisplayDialog("Info", 
                $"Sprite folder not found at {spritePath}.\n\n" +
                "Please create the folder and add your toast sprites, then try again.", "OK");
            return;
        }

        // Load all toast orders
        string[] orderGuids = AssetDatabase.FindAssets("t:ToastOrder", new[] { toastOrderPath });
        int assignedCount = 0;

        foreach (string guid in orderGuids)
        {
            string orderPath = AssetDatabase.GUIDToAssetPath(guid);
            ToastOrder order = AssetDatabase.LoadAssetAtPath<ToastOrder>(orderPath);
            
            if (order != null)
            {
                // Try to find matching sprite
                string orderFileName = Path.GetFileNameWithoutExtension(orderPath);
                string[] possibleSpriteNames = new string[]
                {
                    orderFileName,
                    orderFileName.Replace("_", " "),
                    order.orderName.Replace(" ", "_")
                };

                foreach (string spriteName in possibleSpriteNames)
                {
                    string[] spriteGuids = AssetDatabase.FindAssets(spriteName + " t:Sprite", new[] { spritePath });
                    
                    if (spriteGuids.Length > 0)
                    {
                        string spritePath_found = AssetDatabase.GUIDToAssetPath(spriteGuids[0]);
                        Sprite sprite = AssetDatabase.LoadAssetAtPath<Sprite>(spritePath_found);
                        
                        if (sprite != null)
                        {
                            order.orderIcon = sprite;
                            EditorUtility.SetDirty(order);
                            assignedCount++;
                            Debug.Log($"Assigned sprite to {order.orderName}");
                            break;
                        }
                    }
                }
            }
        }

        AssetDatabase.SaveAssets();
        
        EditorUtility.DisplayDialog("Complete", 
            $"Assigned {assignedCount} out of {orderGuids.Length} sprites!\n\n" +
            "Check the Console for details. Manually assign any missing sprites in the Inspector.", "OK");
    }
}