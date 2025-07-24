using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Fungus;

public class NPCController : MonoBehaviour
{
    [LabelText("Possible Orders")]
    public NPCOrderData[] possibleOrders;
    [ReadOnly, ShowInInspector]public Flowchart currentFlowchart;
    [ReadOnly, ShowInInspector] public NPCOrderData currentOrder;
    public bool isOrderCompleted;
    

    void Start()
    {
        GenerateRandomOrder();
        //Sementara
        GameObject myFlowchart = Instantiate(currentOrder.myFlowchart);
        currentFlowchart = myFlowchart.GetComponent<Flowchart>();
        
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
