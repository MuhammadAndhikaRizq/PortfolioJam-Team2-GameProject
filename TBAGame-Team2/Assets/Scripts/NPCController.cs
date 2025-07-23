using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class NPCController : MonoBehaviour
{
    [LabelText("Possible Orders")]
    public NPCOrderData[] possibleOrders;
    public NPCOrderData currentOrder;
    public bool isOrderCompleted;

    void Start()
    {
        //Sementara
        GenerateRandomOrder();
    }

    void GenerateRandomOrder()
    {
        int randomIndex = Random.Range(0, possibleOrders.Length);
        currentOrder = possibleOrders[randomIndex];
    }

    public bool ReceiveItem(ItemData item)
    {
        return item == currentOrder.requestedItem;
    }
}
