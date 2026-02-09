using UnityEngine;

[CreateAssetMenu(fileName = "New Toast Order", menuName = "Food Truck/Toast Order")]
public class ToastOrder : ScriptableObject
{
    public string orderName;
    public Sprite orderIcon; // Optional: icon for the toast type
    public string description; // e.g., "Avocado Toast", "Butter Toast", etc.
}
