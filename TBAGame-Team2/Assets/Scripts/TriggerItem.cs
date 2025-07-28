using System.Collections;
using System.Collections.Generic;
using Fungus;
using Sirenix.OdinInspector;
using UnityEngine;

public class TriggerItem : MonoBehaviour
{
    [BoxGroup("Script Refrence"), ShowInInspector, ReadOnly]
    NPCController npcController;

    [BoxGroup("Script Refrence"), SerializeField]
    public ItemSelection itemSelection;
    [BoxGroup("Script Refrence"), SerializeField]
    public ItemSelection itemSelection2;

    [BoxGroup("Script Refrence"), SerializeField]
    public OrderCheck orderCheck;

    [FoldoutGroup("Item Counts"), LabelText("Rokok Djarum")]
    public int rokokDjarum;
    [FoldoutGroup("Item Counts"), LabelText("Rokok Gudang")]
    public int rokokGudang;

    [FoldoutGroup("Item Counts"), LabelText("Kopi Kapal")]
    public int kopiKapal;

    [FoldoutGroup("Item Counts"), LabelText("Mie")]
    public int mie;

    [LabelText("Maksimum Item Count")]
    public int maxItemCount;

    [LabelText("Item Count")]
    public int itemCount = 0;

    [LabelText("NPC Controller Detector")]
    public GameObject npcControllerDetector;

    void Start()
    {
        if (itemSelection == null)
        {
            itemSelection = GetComponent<ItemSelection>();
        }
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Item")) return;
        CollectItem(other);
    }

    void Update()
    {
        if (npcController == null) return;

        if (orderCheck != null && orderCheck.firstOrderCompleted && orderCheck.secondOrderCompleted == false)
            
            maxItemCount = npcController.currentSecondOrder.quantity;

        else if (orderCheck != null && orderCheck.secondOrderCompleted == true && orderCheck.thirdOrderCompleted == false)
            maxItemCount = npcController.currentThirdOrder.quantity;
        else
            maxItemCount = npcController.currentOrder.quantity;
    }

    public void SetNPCController(NPCController controller, OrderCheck orderChecker)
    {
        npcController = controller;
        orderCheck = orderChecker;
    }

    public void ClearNPCController()
    {
        npcController = null;
        orderCheck = null;
    }

    void CollectItem(Collider2D other)
    {
        ItemHolder itemHolder = other.GetComponent<ItemHolder>();
        itemSelection2 = other.GetComponent<ItemSelection>();
        if (itemHolder == null || npcController == null) return;

        NPCOrderData data = npcController.currentOrder;

        if (itemCount < maxItemCount)
        {
            ItemData itemData = itemHolder.itemData;

            switch (itemData.itemName)
            {
                case "RokokDjarum":
                    rokokDjarum++;
                    break;
                case "RokokGudang":
                    rokokGudang++;
                    break;
                case "KopiKapal":
                    kopiKapal++;
                    break;
                case "Mie":
                    mie++;
                    break;
                default:
                    return;
            }
            itemSelection2.ResetPosition();

            itemCount++;
            
            Debug.Log("Item collected: " + itemSelection2.name);
        }

        if (itemCount >= maxItemCount)
        {
            itemSelection.enabled = true;
        }
        else
        {
            if (itemSelection.enabled)
            {
                itemSelection.enabled = false;
            }
        }
    }

    public void ResetPlastic()
    {
        rokokDjarum = 0;
        rokokGudang = 0;
        kopiKapal = 0;
        mie = 0;
        itemCount = 0;
        itemSelection.ResetPosition();
        itemSelection.enabled = false;
    }
}