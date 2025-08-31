using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Customer : MonoBehaviour
{
    [Header("Customer Data")]
    public NPCData npcData;
    public Order order;
    
    [Header("UI Elements")]
    public GameObject thoughtBubble;
    public TextMeshProUGUI orderText;
    public Slider patienceSlider;
    
    [Header("Appearance")]
    public SpriteRenderer spriteRenderer;
    public Sprite normalSprite;
    public Sprite happySprite;
    public Sprite angrySprite;
    
    // State variables
    private bool orderFulfilled = false;
    private bool isWaiting = true;
    private float currentPatience;
    private bool itemsCollected = false;
    
    void Start()
    {
        // Generate a random order for this customer
        order = OrderSystem.Instance.GenerateRandomOrder();
        
        // Set up patience
        currentPatience = npcData.maxPatienceBarValue;
        
        // Set up thought bubble with order details
        UpdateOrderDisplay();
        
        // Show thought bubble
        thoughtBubble.SetActive(true);
        
        // Start patience depletion
        StartCoroutine(PatienceCoroutine());
        
        // Log for testing
        Debug.Log("Customer arrived with order. Total: $" + order.TotalPrice);
    }
    
    void UpdateOrderDisplay()
    {
        string orderString = "I want:\n";
        foreach (OrderItem item in order.items)
        {
            orderString += $"{item.quantity}x {item.item.itemName} - ${item.item.itemPrice * item.quantity}\n";
        }
        orderText.text = orderString;
    }
    
    IEnumerator PatienceCoroutine()
    {
        while (isWaiting && currentPatience > 0 && !orderFulfilled)
        {
            // Decrease patience over time
            currentPatience -= Time.deltaTime;
            
            // Update patience slider
            if (patienceSlider != null)
            {
                patienceSlider.value = currentPatience;
                
                // Change color based on patience level
                float patiencePercent = currentPatience / npcData.maxPatienceBarValue;
                Image fillImage = patienceSlider.fillRect.GetComponent<Image>();
                
                if (patiencePercent > 0.6f)
                    fillImage.color = Color.green;
                else if (patiencePercent > 0.3f)
                    fillImage.color = Color.yellow;
                else
                    fillImage.color = Color.red;
            }
            
            // Check if patience ran out
            if (currentPatience <= 0)
            {
                OnPatienceRunOut();
                yield break;
            }
            
            yield return null;
        }
    }
    
    void OnPatienceRunOut()
    {
        // Customer gets angry and leaves
        isWaiting = false;
        spriteRenderer.sprite = angrySprite;
        thoughtBubble.SetActive(false);
        
        // TODO: Implement negative consequences
        Debug.Log("Customer left angry!");
        
        // Wait a moment then leave
        StartCoroutine(LeaveAfterDelay(2f));
    }
    
    // Call this when player has collected all items for this customer
    public void OnAllItemsCollected()
    {
        itemsCollected = true;
        
        // Change thought bubble to show readiness for payment
        orderText.text = "I'm ready to pay!";
        
        // Change customer appearance to happy
        spriteRenderer.sprite = happySprite;
        
        // Stop patience depletion
        isWaiting = false;
        
        Debug.Log("All items collected. Ready for payment.");
    }
    
    // Call this when player interacts with the customer (e.g., clicks on them)
    public void OnCustomerInteracted()
    {
        if (!itemsCollected)
        {
            // Show order reminder
            Debug.Log("Customer says: I'm still waiting for my items!");
            return;
        }
        
        // Start the cashier mini-game
        Cashier.Instance.StartMiniGame(order);
    }
    
    // Call this when the cashier mini-game is completed successfully
    public void OnTransactionCompleted()
    {
        orderFulfilled = true;
        thoughtBubble.SetActive(false);
        
        // TODO: Implement positive consequences (money, reputation, etc.)
        Debug.Log("Transaction completed successfully!");
        
        // Wait a moment then leave
        StartCoroutine(LeaveAfterDelay(2f));
    }
    
    // Call this when the cashier mini-game is failed
    public void OnTransactionFailed()
    {
        // Customer gets angry
        spriteRenderer.sprite = angrySprite;
        
        // TODO: Implement negative consequences
        Debug.Log("Transaction failed!");
        
        // Wait a moment then leave
        StartCoroutine(LeaveAfterDelay(2f));
    }
    
    IEnumerator LeaveAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        
        // Destroy the customer GameObject
        Destroy(gameObject);
    }
    
    void OnMouseDown()
    {
        // Allow clicking on the customer to interact
        OnCustomerInteracted();
    }
}