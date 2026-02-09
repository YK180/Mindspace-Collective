using UnityEngine;
using UnityEngine.SceneManagement;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Extensions;

public class MenuNavigation : MonoBehaviour
{
    private FirebaseAuth auth;
    private DatabaseReference dbReference;

    [Header("Scene Settings")]
    public string loginSceneName = "Raphael_login"; // Exact name of your Login scene

    void Start()
    {
        // Initialize Firebase
        auth = FirebaseAuth.DefaultInstance;
        dbReference = FirebaseDatabase.DefaultInstance.RootReference;
    }

    // Call this function from your 'Quit' button's On Click() event
    public void QuitAndSyncData()
    {
        Debug.Log("Quit process started...");

        if (auth.CurrentUser != null)
        {
            string userId = auth.CurrentUser.UserId;
            string quitTime = System.DateTime.Now.ToString();

            
            dbReference.Child("users").Child(userId).Child("lastQuitTime").SetValueAsync(quitTime).ContinueWithOnMainThread(task => {
                if (task.IsCompleted)
                {
                    Debug.Log("Database updated. Transitioning to login...");
                    ChangeToLoginScene();
                }
                else
                {
                    Debug.LogError("Firebase Update Failed: " + task.Exception);
                    // Still change the scene so the user isn't stuck
                    ChangeToLoginScene();
                }
            });
        }
        else
        {
            // If no user is logged in, just go back to the login page
            ChangeToLoginScene();
        }
    }

    private void ChangeToLoginScene()
    {
        // This closes the app if you're on a headset or build
        // Application.Quit(); 

        // This transitions to the login scene as requested
        SceneManager.LoadScene(loginSceneName);

        // Stops the editor from playing so you can verify the quit action
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}