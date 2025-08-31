using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class Cashier : MonoBehaviour
{
    public static Cashier Instance;
    
    [Header("UI References")]
    public GameObject miniGamePanel;
    public TextMeshProUGUI displayText;
    public TextMeshProUGUI orderSummaryText;
    public TextMeshProUGUI paymentText;
    public TextMeshProUGUI changeText;
    public GameObject numberPad;
    public GameObject cashDrawer;
    public Button confirmButton;
    public Button giveChangeButton;
    
    [Header("Money Settings")]
    public int[] denominations = { 1000, 2000, 5000, 10000, 20000, 50000 };
    public int[] commonNotes = { 10000, 20000, 50000 };
    
    private Order currentOrder;
    private int customerPayment;
    private int calculatedTotal = 0;
    private int changeDue = 0;
    private int selectedChange = 0;
    
    private string currentInput = "";
    private bool isAddingItems = true;
    
    private Customer currentCustomer;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        // Cache the customer reference once
        currentCustomer = FindObjectOfType<Customer>();
    }

    // Add a method to manually start the mini-game for testing
    public void StartTestGame()
    {
        if (OrderSystem.Instance != null)
        {
            Order testOrder = OrderSystem.Instance.GenerateRandomOrder();
            StartMiniGame(testOrder);
        }
    }
    
    public void StartMiniGame(Order order)
    {
        Debug.Log("Starting mini-game with order total: $" + order.TotalPrice);

        currentOrder = order;

        // Determine customer payment (ensure it's at least the order total)
        customerPayment = commonNotes[commonNotes.Length - 1];
        foreach (int note in commonNotes)
        {
            if (note >= currentOrder.TotalPrice)
            {
                customerPayment = note;
                break;
            }
        }

        SetupUI();
        ResetCalculator();
    }
    
    void SetupUI()
    {
        // Display order summary
        System.Text.StringBuilder orderSummaryBuilder = new System.Text.StringBuilder();
        orderSummaryBuilder.Append("Order Summary:\n");
        
        foreach (OrderItem item in currentOrder.items)
        {
            orderSummaryBuilder.Append($"{item.quantity}x {item.item.itemName} - ${item.item.itemPrice * item.quantity}\n");
        }
        
        orderSummaryBuilder.Append($"\nTotal: ${currentOrder.TotalPrice}");
        
        if (orderSummaryText != null)
        {
            orderSummaryText.text = orderSummaryBuilder.ToString();
        }
        
        // Setup payment info
        if (paymentText != null)
            paymentText.text = $"Customer pays: ${customerPayment}";
            
        changeDue = customerPayment - currentOrder.TotalPrice;
        
        if (changeText != null)
            changeText.text = $"Change due: ${changeDue}";
        
        // Setup buttons
        if (confirmButton != null)
        {
            confirmButton.onClick.RemoveAllListeners();
            confirmButton.onClick.AddListener(OnConfirmTotal);
        }
        
        if (giveChangeButton != null)
        {
            giveChangeButton.onClick.RemoveAllListeners();
            giveChangeButton.onClick.AddListener(OnGiveChange);
        }
        
        // Initially hide cash drawer
        if (cashDrawer != null)
            cashDrawer.SetActive(false);
            
        if (giveChangeButton != null)
            giveChangeButton.gameObject.SetActive(false);
    }
    
    void ResetCalculator()
    {
        currentInput = "";
        calculatedTotal = 0;
        isAddingItems = true;
        UpdateDisplay();
    }
    
    public void OnNumberPressed(int number)
    {
        if (!isAddingItems) return;
        
        currentInput += number.ToString();
        UpdateDisplay();
    }
    
    public void OnAddPressed()
    {
        if (!isAddingItems || string.IsNullOrEmpty(currentInput)) return;
        
        int value = int.Parse(currentInput);
        calculatedTotal += value;
        currentInput = "";
        UpdateDisplay();
    }
    
    public void OnEqualsPressed()
    {
        if (string.IsNullOrEmpty(currentInput)) return;
        
        int value = int.Parse(currentInput);
        calculatedTotal += value;
        currentInput = calculatedTotal.ToString();
        isAddingItems = false;
        UpdateDisplay();
    }
    
    public void OnClearPressed()
    {
        currentInput = "";
        UpdateDisplay();
    }
    
    void UpdateDisplay()
    {
        if (displayText != null)
        {
            if (!string.IsNullOrEmpty(currentInput))
            {
                displayText.text = currentInput;
            }
            else
            {
                displayText.text = calculatedTotal.ToString();
            }
        }
    }
    
    void OnConfirmTotal()
    {
        if (calculatedTotal == currentOrder.TotalPrice)
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
        selectedChange += value;
        if (changeText != null)
            changeText.text = $"Change due: ${changeDue}\nSelected: ${selectedChange}";
    }
    
    void OnGiveChange()
    {
        if (selectedChange == changeDue)
        {
            // Correct change given
            Debug.Log("Transaction completed successfully!");
            if (miniGamePanel != null) miniGamePanel.SetActive(false);
            
            // Notify customer of successful transaction
            if (currentCustomer != null)
            {
                currentCustomer.OnTransactionCompleted();
            }
        }
        else
        {
            // Incorrect change - provide feedback
            if (changeText != null)
                changeText.text = $"Incorrect change! Try again\nDue: ${changeDue}, Selected: ${selectedChange}";
            selectedChange = 0;
        }
    }
    
    IEnumerator ResetAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        ResetCalculator();
    }
    
    // For testing - remove in final version
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
}