using UnityEngine;
using TMPro;
using UnityEngine.Events;

public class DayTimeManager : MonoBehaviour
{
    public static DayTimeManager Instance;

    [Header("Time Settings")]
    public int startHour = 9;
    public int endHour = 18;
    public float realSecondsPerGameMinute = 1f;
    
    [Header("UI References")]
    public TextMeshProUGUI clockText;
    
    [Header("Events")]
    public UnityEvent onDayStart;
    public UnityEvent onDayEnd;
    
    private int currentHour;
    private int currentMinute;
    private float gameTimeInSeconds = 0f;
    private bool dayIsActive = true;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        currentHour = startHour;
        onDayStart?.Invoke();
    }

    void Update()
    {
        if (!dayIsActive) return;
        
        gameTimeInSeconds += Time.deltaTime / realSecondsPerGameMinute * 60f;
        int totalGameMinutes = Mathf.FloorToInt(gameTimeInSeconds / 60f);
        currentHour = startHour + (totalGameMinutes / 60);
        currentMinute = totalGameMinutes % 60;
        
        UpdateClockDisplay();
        
        if (currentHour >= endHour) EndDay();
    }

    void UpdateClockDisplay()
    {
        if (clockText != null)
        {
            string period = currentHour >= 12 ? "PM" : "AM";
            int displayHour = currentHour > 12 ? currentHour - 12 : currentHour;
            if (displayHour == 0) displayHour = 12;
            clockText.text = $"{displayHour}:{currentMinute:D2} {period}";
        }
    }

    void EndDay()
    {
        if (!dayIsActive) return;
        dayIsActive = false;
        
        if (ScoreManager.Instance != null) ScoreManager.Instance.SaveScoreAtEndDay();
        
        onDayEnd?.Invoke();
        
        // FIXED: Using FindFirstObjectByType instead of Obsolete FindObjectOfType
        CustomerSpawner spawner = FindFirstObjectByType<CustomerSpawner>();
        if (spawner != null) spawner.StopSpawning();
    }

    public bool IsDayActive() => dayIsActive;
}