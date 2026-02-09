using UnityEngine;
using TMPro;
using Firebase;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Extensions; 
using UnityEngine.SceneManagement; // Required for changing scenes

public class VRFirebaseManager : MonoBehaviour
{
    [Header("UI References")]
    public TMP_InputField emailField;
    public TMP_InputField passwordField;
    public TMP_Text statusText; 

    [Header("Scene Settings")]
    public string nextSceneName = "Raphael_menu";

    private FirebaseAuth auth;
    private DatabaseReference dbReference;

    void Start()
    {
        // Initialize Firebase
        auth = FirebaseAuth.DefaultInstance;
        dbReference = FirebaseDatabase.GetInstance("https://ip-project-2fdf7-default-rtdb.asia-southeast1.firebasedatabase.app/").RootReference;
        
        if (statusText != null) statusText.text = "Ready to Login/Register";
    }

    // 1. REGISTER: Create account and database node ONLY (No Scene Change)
    public void RegisterAndCreateUserNode()
    {
        string email = emailField.text.Trim();
        string password = passwordField.text.Trim();

        if (statusText != null) statusText.text = "Registering...";

        auth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWithOnMainThread(task => {
            if (task.IsCanceled || task.IsFaulted) {
                if (statusText != null) statusText.text = "Reg Failed!";
                Debug.LogError(task.Exception);
                return;
            }

            FirebaseUser newUser = task.Result.User;
            // Create the database entry but stay in the current scene
            UpdateUserDatabase(newUser.UserId, email, "Account Created! Please Log In."); 
        });
    }

    // 2. LOGIN: Authenticate and then Change Scene
    public void Login()
    {
        string email = emailField.text.Trim(); 
        string password = passwordField.text;

        if (statusText != null) statusText.text = "Logging in...";

        auth.SignInWithEmailAndPasswordAsync(email, password).ContinueWithOnMainThread(task => {
            if (task.IsCanceled || task.IsFaulted) {
                if (statusText != null) statusText.text = "Login Failed!";
                return;
            }

            FirebaseUser user = task.Result.User;
            
            // After successful login, update timestamp and change scene
            if (statusText != null) statusText.text = "Success! Entering VR...";
            Debug.Log("Login Successful. Loading: " + nextSceneName);
            
            // Optional: Update last login time before leaving
            UpdateUserDatabase(user.UserId, email, "Loading...");
            
            SceneManager.LoadScene(nextSceneName); 
        });
    }

    private void UpdateUserDatabase(string userId, string email, string message)
    {
        UserData data = new UserData(email);
        string json = JsonUtility.ToJson(data);

        dbReference.Child("users").Child(userId).SetRawJsonValueAsync(json).ContinueWithOnMainThread(dbTask => {
            if (statusText != null && !SceneManager.GetActiveScene().name.Equals(nextSceneName)) 
            {
                statusText.text = message;
            }
            Debug.Log("Database entry updated for: " + userId);
        });
    }
}

[System.Serializable]
public class UserData
{
    public string email;
    public string lastLogin;

    public UserData(string email)
    {
        this.email = email;
        this.lastLogin = System.DateTime.Now.ToString();
    }
}