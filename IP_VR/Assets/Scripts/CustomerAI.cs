using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class CustomerAI : MonoBehaviour
{
    [Header("Navigation")]
    public Transform orderPoint; // The position where customer walks to
    private NavMeshAgent agent;
    
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
        
        // Wait a frame for NavMesh to initialize, then set destination
        if (orderPoint != null && agent != null)
        {
            StartCoroutine(SetDestinationWhenReady());
        }
        
        // Select a random order
        if (possibleOrders.Length > 0)
        {
            currentOrder = possibleOrders[Random.Range(0, possibleOrders.Length)];
        }
    }
    
    IEnumerator SetDestinationWhenReady()
    {
        // Wait for agent to be placed on NavMesh
        yield return new WaitForEndOfFrame();
        
        // Check if agent is on NavMesh before setting destination
        if (agent != null && agent.isOnNavMesh)
        {
            agent.SetDestination(orderPoint.position);
        }
        else
        {
            Debug.LogWarning($"Customer {gameObject.name} not on NavMesh! Check spawn position and NavMesh bake.");
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
        ShowQuestionMark();
    }

    void ShowQuestionMark()
    {
        if (questionMarkPrefab != null && questionMarkInstance == null)
        {
            questionMarkInstance = Instantiate(questionMarkPrefab, transform.position + questionMarkOffset, Quaternion.identity);
            
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
}