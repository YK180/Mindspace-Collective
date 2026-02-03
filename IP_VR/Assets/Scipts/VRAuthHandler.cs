using UnityEngine;
using TMPro;
using Firebase.Auth; // Requires Firebase Auth SDK installed
using System.Threading.Tasks;

public class VRAuthHandler : MonoBehaviour
{
    [Header("UI References")]
    public TMP_InputField emailField;
    public TMP_InputField passwordField;

    private FirebaseAuth auth;

    void Start()
    {
        // Initialize Firebase Auth
        auth = FirebaseAuth.DefaultInstance;
    }

    public void Register()
    {
        string email = emailField.text;
        string password = passwordField.text;

        auth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWith(task => {
            if (task.IsFaulted || task.IsCanceled)
            {
                Debug.LogError("Registration Failed: " + task.Exception);
                return;
            }
            Debug.Log("User Registered: " + task.Result.User.Email);
        });
    }

    public void Login()
    {
        string email = emailField.text;
        string password = passwordField.text;

        auth.SignInWithEmailAndPasswordAsync(email, password).ContinueWith(task => {
            if (task.IsFaulted || task.IsCanceled)
            {
                Debug.LogError("Login Failed: " + task.Exception);
                return;
            }
            Debug.Log("Login Successful!");
            // You can load a new scene here: SceneManager.LoadScene("GameScene");
        });
    }
}