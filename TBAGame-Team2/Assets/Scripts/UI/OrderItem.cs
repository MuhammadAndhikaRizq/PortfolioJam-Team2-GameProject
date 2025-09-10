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