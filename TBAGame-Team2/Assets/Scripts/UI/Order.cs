using System.Collections.Generic;

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

    public List<ItemData> ToItemDataList()
    {
        List<ItemData> result = new List<ItemData>();
        foreach (OrderItem orderItem in items)
        {
            for (int i = 0; i < orderItem.quantity; i++)
            {
                result.Add(orderItem.item);
            }
        }
        return result;
    }
}