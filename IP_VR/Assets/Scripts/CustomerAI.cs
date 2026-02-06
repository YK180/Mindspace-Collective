using UnityEngine;

public class CustomerAI : MonoBehaviour
{
    [Header("Order")]
    public OrderData[] possibleOrders;
    public OrderData chosenOrder;

    [Header("Movement")]
    public Transform standPoint;
    public float moveSpeed = 1.5f;

    private bool hasArrived = false;

    void Start()
    {
        chosenOrder = possibleOrders[Random.Range(0, possibleOrders.Length)];
    }

    void Update()
    {
        if (!hasArrived)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                standPoint.position,
                moveSpeed * Time.deltaTime
            );

            if (Vector3.Distance(transform.position, standPoint.position) < 0.05f)
            {
                hasArrived = true;
            }
        }
    }
}
