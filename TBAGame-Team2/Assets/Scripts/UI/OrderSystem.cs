using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Order
{
    public List<OrderItem> items = new List<OrderItem>();
    public int TotalPrice
    {
        get
        {
            int total = 0;
            foreach (OrderItem item in items)
            {
                total += item.item.itemPrice * item.quantity;
            }
            return total;
        }
    }
}

[System.Serializable]
public class OrderItem
{
    public ItemData item;
    public int quantity;
    
    public OrderItem(ItemData item, int quantity)
    {
        this.item = item;
        this.quantity = quantity;
    }
}

public class OrderSystem : MonoBehaviour
{
    public static OrderSystem Instance;
    
    public ItemData[] availableItems;
    
    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
            
        // Ensure we have items to work with
        if (availableItems == null || availableItems.Length == 0)
        {
            Debug.LogError("No available items assigned to OrderSystem!");
        }
    }
    
    public Order GenerateRandomOrder(int minItems = 1, int maxItems = 5)
    {
        Order order = new Order();
        
        if (availableItems == null || availableItems.Length == 0)
        {
            Debug.LogError("Cannot generate order - no available items!");
            return order;
        }
        
        int itemCount = Random.Range(minItems, maxItems + 1);
        
        for (int i = 0; i < itemCount; i++)
        {
            ItemData randomItem = availableItems[Random.Range(0, availableItems.Length)];
            int quantity = Random.Range(1, 4); // 1-3 of each item
            
            // Check if item already exists in order
            bool itemExists = false;
            foreach (OrderItem orderItem in order.items)
            {
                if (orderItem.item == randomItem)
                {
                    orderItem.quantity += quantity;
                    itemExists = true;
                    break;
                }
            }
            
            if (!itemExists)
            {
                order.items.Add(new OrderItem(randomItem, quantity));
            }
        }
        
        Debug.Log("Generated order with " + order.items.Count + " items. Total: $" + order.TotalPrice);
        return order;
    }
}