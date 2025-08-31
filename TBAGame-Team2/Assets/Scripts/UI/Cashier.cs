using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Cashier : MonoBehaviour
{
    public static Cashier Instance;

    [Header("UI References")]
    public GameObject miniGamePanel;
    public TextMeshProUGUI  displayText;
    public TextMeshProUGUI  orderSummaryText;
    public TextMeshProUGUI  paymentText;
    public TextMeshProUGUI  changeText;
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
        
        miniGamePanel.SetActive(false);
    }
    
    public void StartMiniGame(Order order)
    {
        currentOrder = order;
        currentCustomer = FindObjectOfType<Customer>(); // Since we have only one customer
        
        // Determine customer payment (ensure it's at least the order total)
        customerPayment = commonNotes[Random.Range(0, commonNotes.Length)];
        while (customerPayment < currentOrder.TotalPrice)
        {
            customerPayment = commonNotes[Random.Range(0, commonNotes.Length)];
        }
        
        SetupUI();
        miniGamePanel.SetActive(true);
        ResetCalculator();
    }
    
    void SetupUI()
    {
        // Display order summary
        orderSummaryText.text = "Order Summary:\n";
        foreach (OrderItem item in currentOrder.items)
        {
            orderSummaryText.text += $"{item.quantity}x {item.item.itemName} - ${item.item.itemPrice * item.quantity}\n";
        }
        orderSummaryText.text += $"\nTotal: ${currentOrder.TotalPrice}";
        
        // Setup payment info
        paymentText.text = $"Customer pays: ${customerPayment}";
        changeDue = customerPayment - currentOrder.TotalPrice;
        changeText.text = $"Change due: ${changeDue}";
        
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
        if (!string.IsNullOrEmpty(currentInput))
        {
            displayText.text = currentInput;
        }
        else
        {
            displayText.text = calculatedTotal.ToString();
        }
    }
    
    void OnConfirmTotal()
    {
        if (calculatedTotal == currentOrder.TotalPrice)
        {
            // Correct total - proceed to payment phase
            numberPad.SetActive(false);
            cashDrawer.SetActive(true);
            giveChangeButton.gameObject.SetActive(true);
        }
        else
        {
            // Incorrect total - provide feedback
            displayText.text = "Incorrect! Try again";
            StartCoroutine(ResetAfterDelay(2f));
        }
    }
    
    public void OnDenominationSelected(int value)
    {
        selectedChange += value;
        changeText.text = $"Change due: ${changeDue}\nSelected: ${selectedChange}";
    }
    
    void OnGiveChange()
    {
        if (selectedChange == changeDue)
        {
            // Correct change given
            Debug.Log("Transaction completed successfully!");
            miniGamePanel.SetActive(false);
            
            // Notify customer of successful transaction
            if (currentCustomer != null)
            {
                currentCustomer.OnTransactionCompleted();
            }
        }
        else
        {
            // Incorrect change - provide feedback
            changeText.text = $"Incorrect change! Try again\nDue: ${changeDue}, Selected: ${selectedChange}";
            selectedChange = 0;
        }
    }
    
    IEnumerator ResetAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        ResetCalculator();
    }
}
