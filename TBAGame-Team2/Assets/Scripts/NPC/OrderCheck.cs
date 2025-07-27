using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using JetBrains.Annotations;
public class OrderCheck : MonoBehaviour
{
    [LabelText("Script Reference")]
    TriggerItem plasticBag;
    NPCController npcController;
    NPCOrderData npcOrderData;
    NPCOrderData secondOrderData;

    public bool firstOrderCompleted = false;
    public bool secondOrderCompleted = false;

    void OnTriggerEnter2D(Collider2D other)
    {
        plasticBag = other.GetComponent<TriggerItem>();
        npcController = this.GetComponent<NPCController>();
        npcOrderData = npcController.currentOrder;
        if (other.CompareTag("PlasticBag"))
        {
            NPCTypeCheck();
        }
    }


    void NPCTypeCheck()
    {
        if (npcOrderData.isUnique == false)
        {
            switch (npcOrderData.npcName)
            {
                case "Pelanggan1":
                    break;
                case "Pelanggan2":
                    break;
                case "Pelanggan3":
                    break;
            }
        }
        
        else if (npcOrderData.isUnique == true)
        {
            if (firstOrderCompleted == false)
            {
                switch (npcOrderData.npcName)
                {
                    case "BuMaya":
                        if(plasticBag.rokok == npcOrderData.quantity && firstOrderCompleted == false)
                        {
                            firstOrderCompleted = true;
                            npcController.currentFlowchart.SetIntegerVariable("StoryState", 1);
                            npcController.currentFlowchart.ExecuteBlock("Maya1SecondOrder");
                        }
                        if(plasticBag.rokok == npcOrderData.quantity && firstOrderCompleted == true)
                        {
                            firstOrderCompleted = true;
                        }
                        break;
                    case "PakAgus":
                        Debug.Log("This is a unique NPC order for PakAgus.");
                        break;
                    case "Bayu":
                        Debug.Log("This is a unique NPC order for Bayu.");
                        break;
                }
            }

        }

    }
}
