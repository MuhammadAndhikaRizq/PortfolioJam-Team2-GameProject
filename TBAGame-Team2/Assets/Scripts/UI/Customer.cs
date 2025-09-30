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

    private readonly HashSet<ItemHolder> _counted = new(); //Guard
    private readonly List<ItemHolder> _servedItems = new();

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
        var holder = collision.gameObject.GetComponent<ItemHolder>();
        if (holder == null || _isServed) return;

        if(_counted.Contains(holder)) return; //Guard

        if (orderList.Contains(holder.itemData))
        {
            _counted.Add(holder);
            CollectItem(holder);
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        var holder = collision.gameObject.GetComponent<ItemHolder>();
        if (holder != null) ResetUI(holder);
    }

    void ResetUI(ItemHolder holder)
    {
        if (_counted.Contains(holder))
        {
            _counted.Remove(holder);

            int index = orderList.FindIndex(i => i == holder.itemData);
            if (index >= 0 && index < orderDisplay.Count)
            {
                orderDisplay[index].color = Color.white;
                _collectedItems = Mathf.Max(0, _collectedItems - 1);
            }
        }
    }

    void CollectItem(ItemHolder holder)
    {
        _collectedItems++;

        int index = orderList.FindIndex(i => i == holder.itemData);

        if (index >= 0 && index < orderDisplay.Count)
        {
            orderDisplay[index].color = Color.gray; // tandai slot sesuai item
        }

        if (_collectedItems >= orderList.Count)
        {
            StartCashierMiniGame();
        }

        if (!_servedItems.Contains(holder))
            _servedItems.Add(holder);
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

        if (patienceBar != null)
            patienceBar.gameObject.SetActive(false);

        foreach (Image display in orderDisplay)
        {
            if (display.gameObject.activeSelf)
                display.color = Color.green;
        }
        for (int i = 0; i < _servedItems.Count; i++)
        {
            var holder = _servedItems[i];
            if (holder == null) continue;

            holder.transform.SetParent(null, true);
            Destroy(holder.gameObject);
        }
        _servedItems.Clear();
        _counted.Clear();

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
        int count = Random.Range(1, 6);
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
