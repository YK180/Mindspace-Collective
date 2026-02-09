using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using System.Collections;

public class CustomerAI : MonoBehaviour
{
    [Header("Navigation")]
    public float rotationOffset = 0f; // Adjust if customer faces wrong direction (try 180 if backwards)
    private NavMeshAgent agent;
    private Transform currentDestination;
    private bool isWaitingInQueue = true;
    
    [Header("Order System")]
    public ToastOrder[] possibleOrders;
    private ToastOrder currentOrder;
    
    [Header("UI Elements")]
    public GameObject questionMarkButtonPrefab; // UI Canvas with Button
    private GameObject questionMarkInstance;
    public Vector3 questionMarkOffset = new Vector3(0, 2.5f, 0); // Height above customer head
    
    private bool hasReachedDestination = false;
    private bool orderPlaced = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        
        // Select a random order
        if (possibleOrders.Length > 0)
        {
            currentOrder = possibleOrders[Random.Range(0, possibleOrders.Length)];
        }
        
        // Register with queue manager
        if (CustomerQueueManager.Instance != null)
        {
            CustomerQueueManager.Instance.AddCustomerToQueue(this);
        }
    }
    
    // Called by queue manager to set walking destination
    public void WalkToPosition(Transform destination)
    {
        currentDestination = destination;
        StartCoroutine(SetDestinationWhenReady());
    }
    
    public void WalkToPosition(Vector3 destination)
    {
        StartCoroutine(SetDestinationToPosition(destination));
    }
    
    IEnumerator SetDestinationWhenReady()
    {
        yield return new WaitForEndOfFrame();
        
        if (agent != null && agent.isOnNavMesh && currentDestination != null)
        {
            agent.SetDestination(currentDestination.position);
        }
    }
    
    IEnumerator SetDestinationToPosition(Vector3 destination)
    {
        yield return new WaitForEndOfFrame();
        
        if (agent != null && agent.isOnNavMesh)
        {
            agent.SetDestination(destination);
        }
    }

    void Update()
    {
        // Check if customer has reached the destination
        if (!hasReachedDestination && agent != null && agent.isOnNavMesh && !agent.pathPending)
        {
            if (agent.remainingDistance <= agent.stoppingDistance)
            {
                if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
                {
                    OnReachedDestination();
                }
            }
        }
        
        // Update question mark position to follow customer
        if (questionMarkInstance != null)
        {
            questionMarkInstance.transform.position = transform.position + questionMarkOffset;
            // Make it face the camera
            if (Camera.main != null)
            {
                questionMarkInstance.transform.LookAt(Camera.main.transform);
                questionMarkInstance.transform.Rotate(0, 180, 0); // Flip to face camera correctly
            }
        }
    }

    void OnReachedDestination()
    {
        hasReachedDestination = true;
        
        // Only show question mark if at the order point (not in queue)
        bool isAtOrderPoint = CustomerQueueManager.Instance != null 
            ? CustomerQueueManager.Instance.IsCustomerAtOrderPoint(this)
            : true;
        
        if (isAtOrderPoint)
        {
            isWaitingInQueue = false;
            
            // Make customer face the order point
            Transform faceTarget = CustomerQueueManager.Instance != null 
                ? CustomerQueueManager.Instance.GetOrderPosition() 
                : null;
                
            if (faceTarget != null)
            {
                Vector3 directionToLook = faceTarget.position - transform.position;
                directionToLook.y = 0;
                if (directionToLook != Vector3.zero)
                {
                    Quaternion targetRotation = Quaternion.LookRotation(directionToLook);
                    transform.rotation = targetRotation * Quaternion.Euler(0, rotationOffset, 0);
                }
            }
            
            ShowQuestionMark();
        }
        else
        {
            // Reset for next movement
            hasReachedDestination = false;
        }
    }

    void ShowQuestionMark()
    {
        if (questionMarkButtonPrefab != null && questionMarkInstance == null)
        {
            // Spawn the question mark button
            questionMarkInstance = Instantiate(questionMarkButtonPrefab, transform.position + questionMarkOffset, Quaternion.identity);
            questionMarkInstance.transform.SetParent(transform);
            
            // Hook up the button to call OnQuestionMarkClicked
            Button button = questionMarkInstance.GetComponentInChildren<Button>();
            if (button != null)
            {
                button.onClick.AddListener(OnQuestionMarkClicked);
                Debug.Log("Question mark button created and listener added");
            }
            else
            {
                Debug.LogError("No Button component found in question mark prefab!");
            }
        }
    }

    public void OnQuestionMarkClicked()
    {
        if (!orderPlaced)
        {
            orderPlaced = true;
            
            Debug.Log($"Question mark clicked! Showing order: {currentOrder.orderName}");
            
            // Hide question mark
            if (questionMarkInstance != null)
            {
                Destroy(questionMarkInstance);
            }
            
            // Display order on UI
            if (OrderUIManager.Instance != null)
            {
                OrderUIManager.Instance.DisplayOrder(currentOrder);
            }
        }
    }

    public ToastOrder GetCurrentOrder()
    {
        return currentOrder;
    }
    
    public void LeaveQueue()
    {
        if (CustomerQueueManager.Instance != null)
        {
            CustomerQueueManager.Instance.CustomerFinished(this);
        }
        
        if (questionMarkInstance != null)
        {
            Destroy(questionMarkInstance);
        }
        
        Destroy(gameObject, 0.5f);
    }
}