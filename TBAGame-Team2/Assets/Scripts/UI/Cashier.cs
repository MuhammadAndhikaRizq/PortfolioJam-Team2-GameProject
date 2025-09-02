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

    private Customer _currentCustomer;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

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
        orderSummaryText.text += $"\nTotal: ${_currentOrder.TotalPrice}";

        // Setup payment info
        paymentText.text = $"Customer pays: ${_customerPayment}";
        _changeDue = _customerPayment - _currentOrder.TotalPrice;
        changeText.text = $"Change due: ${_changeDue}";

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
            if (numberPad != null) numberPad.SetActive(false);
            if (cashDrawer != null) cashDrawer.SetActive(true);
            if (giveChangeButton != null) giveChangeButton.gameObject.SetActive(true);
        }
        else
        {
            // Incorrect total - provide feedback
            if (displayText != null) displayText.text = "Incorrect! Try again";
            StartCoroutine(ResetAfterDelay(2f));
        }
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
            miniGamePanel.SetActive(false);
            _isMiniGameActive = false; // Reset flag
            
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