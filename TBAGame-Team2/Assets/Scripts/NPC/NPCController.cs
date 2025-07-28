using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Fungus;

public class NPCController : MonoBehaviour
{
    [LabelText("Possible Orders")]
    public NPCOrderData[] possibleOrders;
    [LabelText("SecondOrder")]
    public NPCOrderData[] secondOrder;
    [ShowInInspector] public Flowchart currentFlowchart;
    [ShowInInspector] public NPCOrderData currentOrder;
    [ShowInInspector] public NPCOrderData currentSecondOrder;
    [ShowInInspector] public NPCOrderData currentThirdOrder;
    public CameraManager cameraManager;
    public bool isOrderCompleted;



    void Awake()
    {
        GenerateRandomOrder();
    }
    public void Start()
    {
        //Sementara
        GameObject flowchartObj = GameObject.Find(currentOrder.myFlowchart);
        if (flowchartObj != null)
        {
            currentFlowchart = flowchartObj.GetComponent<Flowchart>();
        }
        else
        {
            Debug.LogWarning($"Flowchart dengan nama '{currentOrder.myFlowchart}' tidak ditemukan.");
        }
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
