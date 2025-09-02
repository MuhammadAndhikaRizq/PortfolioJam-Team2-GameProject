using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class Cashier : MonoBehaviour
{
    public static Cashier Instance;

    [Header("UI References")]
    [SerializeField] private GameObject miniGamePanel;
    [SerializeField] private TextMeshProUGUI displayText;
    [SerializeField] private TextMeshProUGUI orderSummaryText;
    [SerializeField] private TextMeshProUGUI paymentText;
    [SerializeField] private TextMeshProUGUI changeText;
    [SerializeField] private GameObject numberPad;
    [SerializeField] private GameObject cashDrawer;
    [SerializeField] private Button confirmButton;
    [SerializeField] private Button giveChangeButton;

    [Header("Money Interaction")]
    [SerializeField] private Money moneyHandler;
    //[SerializeField] private Sprite moneySprite;
    [SerializeField] private Transform moneySpawnPoint;

    [Header("Money Settings")]
    [SerializeField] private int[] denominations = { 1000, 2000, 5000, 10000, 20000, 50000 };
    [SerializeField] private int[] commonNotes = { 10000, 20000, 50000 };

    private Order _currentOrder;
    private int _customerPayment;
    private int _calculatedTotal = 0;
    private int _changeDue = 0;
    private int _selectedChange = 0;
    private string _currentInput = "";
    private bool _isAddingItems = true;
    private bool _isMiniGameActive = false;
    private bool _isMoneyRevealed = false;

    private Customer _currentCustomer;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        if (moneyHandler != null)
        {
            moneyHandler.gameObject.SetActive(false);
        }

        // Start with the panel disabled
        miniGamePanel.SetActive(false);
    }

    void Update()
    {
        // Check if customer ran out of patience during mini-game
        if (miniGamePanel.activeSelf && _currentCustomer != null && _currentCustomer.IsOutOfPatience())
        {
            // Customer got impatient during transaction
            CloseMiniGame();
            Debug.Log("Customer got impatient and left!");
        }
    }

    public void StartMiniGame(Order order, Customer customer)
    {
        if (_isMiniGameActive || customer.IsServed()) return;
    
        _isMiniGameActive = true;
        _currentOrder = order;
        _currentCustomer = customer;

        // Determine customer payment
        _customerPayment = commonNotes[commonNotes.Length - 1]; // Start with largest note

        foreach (int note in commonNotes)
        {
            if (note >= _currentOrder.TotalPrice)
            {
                _customerPayment = note;
                break;
            }
        }

        // Fallback: if still not enough, use multiple of the largest note
        if (_customerPayment < _currentOrder.TotalPrice)
        {
            int multiplier = Mathf.CeilToInt((float)_currentOrder.TotalPrice / _customerPayment);
            _customerPayment *= multiplier;
            Debug.LogWarning($"Order total exceeds largest note. Customer will pay {multiplier}x {_customerPayment/multiplier}");
        }

        SetupUI();
        miniGamePanel.SetActive(true);
        ResetCalculator();
    }

    void SetupUI()
    {
        // Display order summary
        orderSummaryText.text = "Order Summary:\n";
        foreach (OrderItem item in _currentOrder.items)
        {
            orderSummaryText.text += $"{item.quantity}x {item.item.itemName} - ${item.item.itemPrice * item.quantity}\n";
        }

        // Initially hide payment and change info until money is revealed
        paymentText.text = "Confirm total to see payment";
        changeText.text = "";
        _changeDue = _customerPayment - _currentOrder.TotalPrice;
        moneyHandler.gameObject.SetActive(true);

        // Setup buttons
        confirmButton.onClick.RemoveAllListeners();
        confirmButton.onClick.AddListener(OnConfirmTotal);

        giveChangeButton.onClick.RemoveAllListeners();
        giveChangeButton.onClick.AddListener(OnGiveChange);

        // Initially hide cash drawer
        cashDrawer.SetActive(false);
        giveChangeButton.gameObject.SetActive(false);
    }

    void ResetCalculator()
    {
        _currentInput = "";
        _calculatedTotal = 0;
        _isAddingItems = true;
        UpdateDisplay();
    }

    public void OnNumberPressed(int number)
    {
        if (!_isAddingItems) return;

        _currentInput += number.ToString();
        UpdateDisplay();
    }

    public void OnAddPressed()
    {
        if (!_isAddingItems || string.IsNullOrEmpty(_currentInput)) return;

        int value = int.Parse(_currentInput);
        _calculatedTotal += value;
        _currentInput = "";
        UpdateDisplay();
    }

    public void OnEqualsPressed()
    {
        if (string.IsNullOrEmpty(_currentInput)) return;

        int value = int.Parse(_currentInput);
        _calculatedTotal += value;
        _currentInput = _calculatedTotal.ToString();
        _isAddingItems = false;
        UpdateDisplay();
    }

    public void OnClearPressed()
    {
        _currentInput = "";
        UpdateDisplay();
    }

    void UpdateDisplay()
    {
        if (displayText != null)
        {
            if (!string.IsNullOrEmpty(_currentInput))
            {
                displayText.text = _currentInput;
            }
            else
            {
                displayText.text = _calculatedTotal.ToString();
            }
        }
    }

    void OnConfirmTotal()
    {
        if (_calculatedTotal == _currentOrder.TotalPrice)
        {
            // Correct total - proceed to payment phase
            // Spawn money on counter
            SpawnMoneyOnCounter();
            
            // Update payment text to instruct player
            if (paymentText != null)
                paymentText.text = "Click on the money to see how much customer gave you";
        }
        else
        {
            // Incorrect total - provide feedback
            if (displayText != null) displayText.text = "Incorrect! Try again";
            StartCoroutine(ResetAfterDelay(1f));
        }
    }

    void SpawnMoneyOnCounter()
    {
        if (moneyHandler != null && moneySpawnPoint != null)
        {
            // Position the money at the spawn point
            moneyHandler.transform.position = moneySpawnPoint.position;
            
            // Set up the money with the correct amount
            moneyHandler.Setup(_customerPayment, null); // Change null into money sprite when ready

            // Show the money
            moneyHandler.gameObject.SetActive(true);
            moneyHandler.Show();
            
            // Subscribe to the reveal event
            moneyHandler.OnMoneyRevealed += OnMoneyRevealed;
            
            _isMoneyRevealed = false;
        }
    }

    void OnMoneyRevealed(int amount)
    {
        _isMoneyRevealed = true;
        
        // Update payment text to show the amount
        if (paymentText != null)
            paymentText.text = $"Customer pays: ${amount}";
        
        // Show the change due
        if (changeText != null)
            changeText.text = $"Change due: ${_changeDue}";
        
        // Show the cash drawer and change button
        if (cashDrawer != null) cashDrawer.SetActive(true);
        if (giveChangeButton != null) giveChangeButton.gameObject.SetActive(true);
    }

    public void OnDenominationSelected(int value)
    {
        _selectedChange += value;
        if (changeText != null)
            changeText.text = $"Change due: ${_changeDue}\nSelected: ${_selectedChange}";
    }

    void OnGiveChange()
    {
        if (_selectedChange == _changeDue)
        {
            // Correct change given
            Debug.Log("Transaction completed successfully!");
            
            // Hide the money
            if (moneyHandler != null)
            {
                moneyHandler.Hide();
                moneyHandler.OnMoneyRevealed -= OnMoneyRevealed;
            }
            
            miniGamePanel.SetActive(false);
            
            // Notify customer of successful transaction
            if (_currentCustomer != null)
            {
                _currentCustomer.TransactionCompleted(true);
            }
        }
        else
        {
            // Incorrect change - provide feedback
            changeText.text = $"Incorrect change! Try again\nDue: ${_changeDue}, Selected: ${_selectedChange}";
            _selectedChange = 0;
        }
    }

    IEnumerator ResetAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        ResetCalculator();
    }

    public void CloseMiniGame()
    {
        // Hide the money if visible
        if (moneyHandler != null && moneyHandler.gameObject.activeSelf)
        {
            moneyHandler.Hide();
            moneyHandler.OnMoneyRevealed -= OnMoneyRevealed;
        }
        
        miniGamePanel.SetActive(false);
        _isMiniGameActive = false;
    }

    // For testing - remove in final version
    #if UNITY_EDITOR
    void OnGUI()
    {
        // Add a test button to hide the mini-game
        if (GUI.Button(new Rect(10, 10, 150, 30), "Hide Mini-Game"))
        {
            if (miniGamePanel != null)
                miniGamePanel.SetActive(false);
        }

        // Add a test button to show the mini-game
        if (GUI.Button(new Rect(10, 50, 150, 30), "Show Mini-Game"))
        {
            if (miniGamePanel != null)
                miniGamePanel.SetActive(true);
        }
    }
    #endif
}