using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class CustomerAI : MonoBehaviour
{
    [Header("Navigation")]
    public Transform orderPoint; // The position where customer walks to (set by spawner or queue)
    public float rotationOffset = 0f; // Adjust if customer faces wrong direction (try 180 if backwards)
    private NavMeshAgent agent;
    private Transform currentDestination; // Current target position
    private bool isWaitingInQueue = true; // Whether customer is still in queue
    
    [Header("Order System")]
    public ToastOrder[] possibleOrders; // Array of all possible toast orders
    private ToastOrder currentOrder;
    
    [Header("UI Elements")]
    public GameObject questionMarkPrefab; // The question mark UI prefab
    private GameObject questionMarkInstance;
    public Canvas worldCanvas; // Canvas that follows the customer
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
        else
        {
            // Fallback: if no queue manager, go directly to order point
            if (orderPoint != null)
            {
                StartCoroutine(SetDestinationWhenReady());
            }
        }
    }
    
    // Called by queue manager to set walking destination
    public void WalkToPosition(Transform destination)
    {
        currentDestination = destination;
        StartCoroutine(SetDestinationWhenReady());
    }
    
    // Overload for position instead of transform
    public void WalkToPosition(Vector3 destination)
    {
        StartCoroutine(SetDestinationToPosition(destination));
    }
    
    IEnumerator SetDestinationToPosition(Vector3 destination)
    {
        yield return new WaitForEndOfFrame();
        
        if (agent != null && agent.isOnNavMesh)
        {
            agent.SetDestination(destination);
            Debug.Log($"Setting destination to position: {destination}");
        }
    }
    
    IEnumerator SetDestinationWhenReady()
    {
        // Wait for agent to be placed on NavMesh
        yield return new WaitForEndOfFrame();
        
        // Use current destination if set, otherwise fall back to orderPoint
        Transform targetPoint = currentDestination != null ? currentDestination : orderPoint;
        
        // Debug info
        Debug.Log($"Customer spawned at: {transform.position}");
        Debug.Log($"Agent is on NavMesh: {(agent != null && agent.isOnNavMesh)}");
        Debug.Log($"Target point position: {(targetPoint != null ? targetPoint.position.ToString() : "NULL")}");
        
        // Check if agent is on NavMesh before setting destination
        if (agent != null && agent.isOnNavMesh)
        {
            if (targetPoint != null)
            {
                agent.SetDestination(targetPoint.position);
                Debug.Log($"Setting destination to: {targetPoint.position}");
            }
        }
        else
        {
            Debug.LogWarning($"Customer {gameObject.name} not on NavMesh! Check spawn position and NavMesh bake.");
            if (agent == null) Debug.LogError("NavMeshAgent is NULL!");
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
        }
    }

    void OnReachedDestination()
    {
        hasReachedDestination = true;
        Debug.Log($"Customer {gameObject.name} reached destination!");
        
        // Only show question mark if at the order point (not in queue)
        bool isAtOrderPoint = CustomerQueueManager.Instance != null 
            ? CustomerQueueManager.Instance.IsCustomerAtOrderPoint(this)
            : true; // If no queue manager, assume they're at order point
        
        if (isAtOrderPoint)
        {
            Debug.Log("Customer is at order point. Showing question mark.");
            isWaitingInQueue = false;
            
            // Make customer face the order point (or food truck)
            Transform faceTarget = CustomerQueueManager.Instance != null 
                ? CustomerQueueManager.Instance.GetOrderPosition() 
                : orderPoint;
                
            if (faceTarget != null)
            {
                Vector3 directionToLook = faceTarget.position - transform.position;
                directionToLook.y = 0; // Keep it horizontal
                if (directionToLook != Vector3.zero)
                {
                    Quaternion targetRotation = Quaternion.LookRotation(directionToLook);
                    // Apply offset if model faces wrong direction
                    transform.rotation = targetRotation * Quaternion.Euler(0, rotationOffset, 0);
                }
            }
            
            ShowQuestionMark();
        }
        else
        {
            Debug.Log("Customer is waiting in queue.");
            // Reset for next movement
            hasReachedDestination = false;
        }
    }

    void ShowQuestionMark()
    {
        Debug.Log($"ShowQuestionMark called. Prefab is null: {questionMarkPrefab == null}");
        
        if (questionMarkPrefab != null && questionMarkInstance == null)
        {
            questionMarkInstance = Instantiate(questionMarkPrefab, transform.position + questionMarkOffset, Quaternion.identity);
            Debug.Log($"Question mark instantiated at: {questionMarkInstance.transform.position}");
            
            // Make it face the camera
            questionMarkInstance.transform.SetParent(transform);
            
            // Add click detection (using VR version for XR Interaction Toolkit)
            QuestionMarkClickVR clickHandler = questionMarkInstance.GetComponent<QuestionMarkClickVR>();
            if (clickHandler == null)
            {
                clickHandler = questionMarkInstance.AddComponent<QuestionMarkClickVR>();
            }
            clickHandler.customer = this;
        }
        else
        {
            if (questionMarkPrefab == null) Debug.LogError("Question Mark Prefab is NULL! Assign it in CustomerSpawner.");
            if (questionMarkInstance != null) Debug.LogWarning("Question mark already exists.");
        }
    }

    public void OnQuestionMarkClicked()
    {
        if (!orderPlaced)
        {
            orderPlaced = true;
            
            // Hide question mark
            if (questionMarkInstance != null)
            {
                Destroy(questionMarkInstance);
            }
            
            // Display order on UI
            OrderUIManager.Instance.DisplayOrder(currentOrder);
        }
    }

    public ToastOrder GetCurrentOrder()
    {
        return currentOrder;
    }
    
    // Call this when customer's order is complete and they should leave
    public void LeaveQueue()
    {
        // Notify queue manager
        if (CustomerQueueManager.Instance != null)
        {
            CustomerQueueManager.Instance.CustomerFinished(this);
        }
        
        // Hide question mark
        if (questionMarkInstance != null)
        {
            Destroy(questionMarkInstance);
        }
        
        // Destroy customer (or make them walk away)
        Destroy(gameObject, 0.5f);
    }
}