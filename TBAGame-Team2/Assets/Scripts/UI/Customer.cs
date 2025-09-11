using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class Customer : MonoBehaviour
{
    [Header("Data")]
    public List<ItemData> menuPool;
    public List<Image> orderDisplay;
    public NPCData data;
    ItemHolder itemHolder;

    [Header("Patience")]
    [SerializeField] private Slider patienceBar;
    [SerializeField] private Image fillImage;
    private float _patience;
    private float _baseDecay = 1;

    [Header("Animation")]
    [SerializeField] private Animator animator;

    [Header("Runtime")]
    public List<ItemData> orderList = new();

    private int _expectedTotal;
    private int _collectedItems = 0;
    private bool _isInMiniGame = false;
    private bool _isServed = false;

    public System.Action OnLeave;

    void Start()
    {
        _patience = data.maxPatienceBarValue;
        MakeOrder();
        ShowOrder();
    }

    void Update()
    {
        // Always decrease _patience, even during mini-game
        if (!_isServed && _patience > 0)
        {
            _patience -= _baseDecay * Time.deltaTime;
            UpdatePatienceUI();

            if (_patience <= 0)
            {
                Leave();
            }
        }
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        itemHolder = collision.gameObject.GetComponent<ItemHolder>();
        if (_collectedItems < orderList.Count && !_isServed)
        {
            for (int i = 0; i <= orderList.Count; i++)
            {
                if (itemHolder != null && itemHolder.itemData == orderList[_collectedItems])
                {
                    CollectItem();
                    break;
                }
            }
        }
    }

    void CollectItem()
    {
        _collectedItems++;

        // Visual feedback for collected item
        if (_collectedItems <= orderDisplay.Count)
        {
            orderDisplay[_collectedItems - 1].color = new Color(0.5f, 0.5f, 0.5f, 0.5f); // Gray out collected item
        }

        // Check if all items are collected
        if (_collectedItems >= orderList.Count)
        {
            StartCashierMiniGame();
        }
    }

    void StartCashierMiniGame()
    {
        _isInMiniGame = true;

        // Convert orderList to the format expected by Cashier
        Order cashierOrder = new Order();
        var groupedItems = orderList.GroupBy(item => item);

        foreach (var group in groupedItems)
        {
            cashierOrder.items.Add(new OrderItem(group.Key, group.Count()));
        }

        // Start the mini-game
        Cashier.Instance.StartMiniGame(cashierOrder, this);
    }

    void UpdatePatienceUI()
    {
        patienceBar.value = _patience / data.maxPatienceBarValue;

        if (patienceBar.value < 0.3f)
        {
            fillImage.color = Color.red;
            animator.SetBool("Rage", false);
            animator.SetBool("Super Rage", true);
        }
        else if (patienceBar.value < 0.5f)
        {
            fillImage.color = Color.yellow;
            animator.SetBool("Rage", true);
        }
        else
        {
            fillImage.color = Color.green;
        }
    }

    public void TransactionCompleted(bool success)
    {
        _isInMiniGame = false;

        if (success)
        {
            _isServed = true;
            ServeSuccess();
        }
        else
        {
            Leave();
        }
    }

    void ServeSuccess()
    {
        animator.SetBool("Happy", true);
        Debug.Log("Customer served successfully!");
        
        // Additional visual feedback
        if (patienceBar != null)
        {
            patienceBar.gameObject.SetActive(false); // Hide patience bar
        }
        
        // Change order display to show served status
        foreach (Image display in orderDisplay)
        {
            if (display.gameObject.activeSelf)
            {
                display.color = Color.green; // Green checkmark effect
            }
        }
        // Wait a moment before leaving
        StartCoroutine(LeaveAfterDelay(2f));
    }

    public bool IsServed()
    {
        return _isServed;
    }

    public bool IsOutOfPatience()
    {
        return _patience <= 0;
    }

    void Leave()
    {
        // Angry customer
        animator.SetBool("Super Rage", true);
        Debug.Log("Customer left angry!");

        // Close mini-game if it's open
        if (Cashier.Instance != null)
        {
            Cashier.Instance.CloseMiniGame();
        }

        // Wait a moment before leaving
        StartCoroutine(LeaveAfterDelay(2f));
    }

    IEnumerator LeaveAfterDelay(float delay)
    {
        //NPC ILANG
        yield return new WaitForSeconds(delay);

        OnLeave?.Invoke();
        Destroy(gameObject);
    }

    void MakeOrder()
    {
        int count = Random.Range(1, 4);
        for (int i = 0; i < count; i++)
        {
            orderList.Add(menuPool[Random.Range(0, menuPool.Count)]);
        }
        _expectedTotal = orderList.Sum(i => i.itemPrice);
    }

    void ShowOrder()
    {
        // Clear all displays first
        foreach (Image display in orderDisplay)
        {
            display.gameObject.SetActive(false);
        }
        
        // Group items by type and show with quantity indicators
        var groupedItems = orderList.GroupBy(item => item);
        int displayIndex = 0;
        
        foreach (var group in groupedItems)
        {
            if (displayIndex >= orderDisplay.Count) break;
            
            orderDisplay[displayIndex].sprite = group.Key.itemIcon;
            orderDisplay[displayIndex].gameObject.SetActive(true);
            
            // Add quantity text (you'll need to add Text components to your order displays)
            Text quantityText = orderDisplay[displayIndex].GetComponentInChildren<Text>();
            if (quantityText != null)
            {
                quantityText.text = group.Count().ToString();
            }
            
            displayIndex++;
        }
    }
}
