using UnityEngine;
using System.Collections.Generic;

public class CustomerQueueManager : MonoBehaviour
{
    public static CustomerQueueManager Instance;

    [Header("Queue Settings")]
    public Transform[] queuePositions; // Array of positions where customers wait in line
    public Transform orderPosition; // The front position where customer orders
    public float queueSpacing = 1.5f; // Distance between customers in queue
    
    [Header("Queue State")]
    private Queue<CustomerAI> waitingCustomers = new Queue<CustomerAI>();
    private CustomerAI currentCustomer; // Customer currently at the order point
    private bool isOrderPointOccupied = false;

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

    // Called when a customer spawns
    public void AddCustomerToQueue(CustomerAI customer)
    {
        if (currentCustomer == null && !isOrderPointOccupied)
        {
            // No one in line, go straight to order point
            currentCustomer = customer;
            isOrderPointOccupied = true;
            customer.WalkToPosition(orderPosition);
        }
        else
        {
            // Add to queue
            waitingCustomers.Enqueue(customer);
            UpdateQueuePositions();
        }
    }

    // Update positions of all customers in queue
    void UpdateQueuePositions()
    {
        int index = 0;
        foreach (CustomerAI customer in waitingCustomers)
        {
            Vector3 queuePos;
            
            if (queuePositions != null && index < queuePositions.Length)
            {
                // Use predefined queue positions
                queuePos = queuePositions[index].position;
            }
            else
            {
                // Generate queue position based on spacing
                queuePos = orderPosition.position + (-orderPosition.forward * queueSpacing * (index + 1));
            }
            
            customer.WalkToPosition(queuePos);
            index++;
        }
    }

    // Called when current customer finishes ordering and leaves
    public void CustomerFinished(CustomerAI customer)
    {
        if (customer == currentCustomer)
        {
            currentCustomer = null;
            isOrderPointOccupied = false;
            
            // Move next customer to order point
            MoveNextCustomerToOrderPoint();
        }
    }

    void MoveNextCustomerToOrderPoint()
    {
        if (waitingCustomers.Count > 0)
        {
            currentCustomer = waitingCustomers.Dequeue();
            isOrderPointOccupied = true;
            currentCustomer.WalkToPosition(orderPosition);
            
            // Update remaining queue positions
            UpdateQueuePositions();
        }
    }

    // Check if customer is at the front of the line
    public bool IsCustomerAtOrderPoint(CustomerAI customer)
    {
        return customer == currentCustomer;
    }

    public Transform GetOrderPosition()
    {
        return orderPosition;
    }
}