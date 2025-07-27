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

    [FoldoutGroup("Item Counts"), LabelText("Rokok")]
    public int rokok;

    [FoldoutGroup("Item Counts"), LabelText("Aqua")]
    public int aqua;

    [FoldoutGroup("Item Counts"), LabelText("Mie")]
    public int mie;

    [LabelText("Maksimum Item Count")]
    public int maxItemCount;

    [LabelText("Item Count")]
    public int itemCount = 0;

    void Start()
    {
        if (itemSelection == null)
        {
            itemSelection = GetComponent<ItemSelection>();
        }

        npcController = FindObjectOfType<NPCController>();
        if (npcController == null)
        {
            Debug.LogWarning("NPCController not found in the scene!");
        }
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Item")) return;
        CollectItem(other);
    }

    void CollectItem(Collider2D other)
    {
        ItemHolder itemHolder = other.GetComponent<ItemHolder>();
        if (itemHolder == null || npcController == null) return;

        NPCOrderData data = npcController.currentOrder;
        maxItemCount = data.quantity;

        if (itemCount < maxItemCount)
        {
            ItemData itemData = itemHolder.itemData;

            switch (itemData.itemName)
            {
                case "Rokok":
                    rokok++;
                    break;
                case "Aqua":
                    aqua++;
                    break;
                case "Mie":
                    mie++;
                    break;
                default:
                    return;
            }

            itemCount++;
            Destroy(other.gameObject);
            Debug.Log("Item collected: " + itemData.itemName);
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
}