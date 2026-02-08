using UnityEngine;
using System.Collections;

public class CustomerSpawner : MonoBehaviour
{
    [Header("Customer Models")]
    public GameObject[] customerPrefabs; // Your 4 customer model prefabs
    
    [Header("Spawn Settings")]
    public Transform spawnPoint; // Where customers spawn
    public Transform orderPoint; // Where customers walk to
    public float spawnInterval = 10f; // Time between spawns
    public int maxCustomers = 3; // Max customers at once
    
    [Header("Order Settings")]
    public ToastOrder[] availableOrders; // All possible toast orders
    
    [Header("UI")]
    public GameObject questionMarkPrefab;
    
    private int currentCustomerCount = 0;

    void Start()
    {
        StartCoroutine(SpawnCustomers());
    }

    IEnumerator SpawnCustomers()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);
            
            if (currentCustomerCount < maxCustomers)
            {
                SpawnRandomCustomer();
            }
        }
    }

    public void SpawnRandomCustomer()
    {
        if (customerPrefabs.Length == 0)
        {
            Debug.LogWarning("No customer prefabs assigned!");
            return;
        }
        
        // Pick random customer model
        GameObject randomCustomerPrefab = customerPrefabs[Random.Range(0, customerPrefabs.Length)];
        
        // Spawn customer
        GameObject customer = Instantiate(randomCustomerPrefab, spawnPoint.position, spawnPoint.rotation);
        
        // Setup CustomerAI component
        CustomerAI customerAI = customer.GetComponent<CustomerAI>();
        if (customerAI == null)
        {
            customerAI = customer.AddComponent<CustomerAI>();
        }
        
        // Assign settings
        customerAI.orderPoint = orderPoint;
        customerAI.possibleOrders = availableOrders;
        customerAI.questionMarkPrefab = questionMarkPrefab;
        
        currentCustomerCount++;
        
        // Optional: Track when customer leaves to decrement count
        StartCoroutine(TrackCustomer(customer));
    }

    IEnumerator TrackCustomer(GameObject customer)
    {
        // Wait until customer is destroyed (when they leave/complete order)
        while (customer != null)
        {
            yield return new WaitForSeconds(1f);
        }
        
        currentCustomerCount--;
    }

    // Call this when customer completes their order and leaves
    public void CustomerLeaving()
    {
        currentCustomerCount--;
    }
}