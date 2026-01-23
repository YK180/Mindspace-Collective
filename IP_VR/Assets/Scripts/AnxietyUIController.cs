using UnityEngine;
using TMPro;

public class AnxietyUIController : MonoBehaviour
{
    public TextMeshProUGUI mainOrder;
    public TextMeshProUGUI[] ghostOrders;

    [Range(0f, 1f)]
    public float anxietyLevel = 0f;

    private float messUpTimer = 0f;
    public float messUpInterval = 0.5f; // controls speed (SAFE)

    void Update()
    {
        if (anxietyLevel > 0.5f)
        {
            messUpTimer += Time.deltaTime;

            if (messUpTimer >= messUpInterval)
            {
                MessUpUI();
                messUpTimer = 0f;
            }
        }
        else
        {
            CalmUI();
        }
    }

    void MessUpUI()
    {
        foreach (var ghost in ghostOrders)
        {
            ghost.gameObject.SetActive(true);

            ghost.rectTransform.anchoredPosition =
                mainOrder.rectTransform.anchoredPosition +
                new Vector2(
                    Random.Range(-8f, 8f),
                    Random.Range(-8f, 8f)
                );
        }
    }

    void CalmUI()
    {
        foreach (var ghost in ghostOrders)
        {
            ghost.gameObject.SetActive(false);
        }
    }
}
