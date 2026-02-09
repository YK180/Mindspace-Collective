using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class DayTimeManager : MonoBehaviour
{
    public static DayTimeManager Instance;

    [Header("Time Settings")]
    public int startHour = 9; // 9 AM
    public int endHour = 18; // 6 PM
    public float realSecondsPerGameMinute = 1f; // 1 real second = 1 game minute
    // This means 60 real seconds = 60 game minutes = 1 game hour
    
    [Header("UI References")]
    public TextMeshProUGUI clockText; // The clock display text
    
    [Header("Events")]
    public UnityEvent onDayStart;
    public UnityEvent onDayEnd;
    
    [Header("Current Time")]
    public int currentHour = 9;
    public int currentMinute = 0;
    
    private float gameTimeInSeconds = 0f;
    private bool dayIsActive = true;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        // Initialize time
        currentHour = startHour;
        currentMinute = 0;
        gameTimeInSeconds = 0f; // Start at 0, we'll calculate from startHour differently
        
        UpdateClockDisplay();
        
        // Start the day
        onDayStart?.Invoke();
        Debug.Log($"Day started at {currentHour}:00");
    }

    void Update()
    {
        if (!dayIsActive) return;
        
        // Advance game time (in seconds)
        gameTimeInSeconds += Time.deltaTime / realSecondsPerGameMinute * 60f;
        
        // Convert game time to hours and minutes from start of day
        int totalGameMinutes = Mathf.FloorToInt(gameTimeInSeconds / 60f);
        currentHour = startHour + (totalGameMinutes / 60);
        currentMinute = totalGameMinutes % 60;
        
        // Update clock display
        UpdateClockDisplay();
        
        // Check if day has ended
        if (currentHour >= endHour)
        {
            EndDay();
        }
    }

    void UpdateClockDisplay()
    {
        if (clockText != null)
        {
            // Format time as HH:MM AM/PM
            string period = currentHour >= 12 ? "PM" : "AM";
            int displayHour = currentHour > 12 ? currentHour - 12 : currentHour;
            if (displayHour == 0) displayHour = 12; // Handle midnight/noon
            
            clockText.text = $"{displayHour}:{currentMinute:D2} {period}";
        }
    }

    void EndDay()
    {
        if (!dayIsActive) return; // Already ended
        
        dayIsActive = false;
        Debug.Log($"Day ended at {currentHour}:00");
        
        // Trigger end of day event
        onDayEnd?.Invoke();
        
        // Stop spawning customers
        if (CustomerSpawner.FindObjectOfType<CustomerSpawner>() != null)
        {
            CustomerSpawner spawner = FindObjectOfType<CustomerSpawner>();
            spawner.StopSpawning();
        }
    }

    public bool IsDayActive()
    {
        return dayIsActive;
    }
    
    public string GetCurrentTimeString()
    {
        string period = currentHour >= 12 ? "PM" : "AM";
        int displayHour = currentHour > 12 ? currentHour - 12 : currentHour;
        if (displayHour == 0) displayHour = 12;
        return $"{displayHour}:{currentMinute:D2} {period}";
    }
    
    // Helper function
    float HourToSeconds(int hour)
    {
        return hour * 3600f;
    }

    // Public method to restart the day (optional)
    public void RestartDay()
    {
        currentHour = startHour;
        currentMinute = 0;
        gameTimeInSeconds = 0f;
        dayIsActive = true;
        UpdateClockDisplay();
        onDayStart?.Invoke();
    }
}