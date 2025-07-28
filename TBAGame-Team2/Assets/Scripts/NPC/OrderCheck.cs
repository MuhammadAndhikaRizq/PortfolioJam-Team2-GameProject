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
    NPCOrderData thirdOrderData;

    public bool firstOrderCompleted = false;
    public bool secondOrderCompleted = false;
    public bool thirdOrderCompleted = false;

    void OnTriggerEnter2D(Collider2D other)
    {
        plasticBag = other.GetComponent<TriggerItem>();
        npcController = this.GetComponent<NPCController>();
        npcOrderData = npcController.currentOrder;
        thirdOrderData = npcController.currentThirdOrder;
        secondOrderData = npcController.currentSecondOrder;
        if (other.CompareTag("PlasticBag"))
        {
            NPCTypeCheck();
            Debug.Log("NPC name : " + npcOrderData.npcName);
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
                        if (plasticBag.rokokDjarum == npcOrderData.quantity && firstOrderCompleted == false)
                        {
                            plasticBag.ResetPlastic();
                            firstOrderCompleted = true;
                            npcController.currentFlowchart.SetIntegerVariable("StoryState", 1);
                            npcController.currentFlowchart.ExecuteBlock("Maya1SecondOrder");
                        }
                        break;
                    case "PakAgus":
                        if (plasticBag.rokokGudang == npcOrderData.quantity)
                        {
                            Debug.Log("Pak Agus's first order completed.");
                            plasticBag.ResetPlastic();
                            npcController.currentFlowchart.SetIntegerVariable("StoryState", 1);
                            npcController.currentFlowchart.ExecuteBlock("Agus1End");
                        }
                        break;
                    case "Bayu":
                        if (plasticBag.kopiKapal == npcOrderData.quantity && firstOrderCompleted == false)
                        {
                            Debug.Log("Bayu's first order completed.");
                            plasticBag.ResetPlastic();
                            firstOrderCompleted = true;
                            npcController.currentFlowchart.SetIntegerVariable("StoryState", 1);
                            npcController.currentFlowchart.ExecuteBlock("Bayu1SecondOrder");
                        }
                        break;
                }
            }
            else if (firstOrderCompleted == true && secondOrderCompleted == false)
            {
                switch (secondOrderData.npcName)
                {
                    case "BuMaya":
                        if (plasticBag.kopiKapal == secondOrderData.quantity && secondOrderCompleted == false)
                        {
                            Debug.Log("Bu Maya's second order completed.");
                            plasticBag.ResetPlastic();
                            npcController.currentFlowchart.SetIntegerVariable("StoryState", 2);
                            npcController.currentFlowchart.ExecuteBlock("Maya1End");
                        }
                        break;
                    case "PakAgus":
                        Debug.Log("This is a unique NPC order for PakAgus.");
                        break;
                    case "Bayu":
                        if (plasticBag.kopiKapal == secondOrderData.quantity && firstOrderCompleted == true && secondOrderCompleted == false)
                        {
                            Debug.Log("Bayu's second order completed.");
                            plasticBag.ResetPlastic();
                            secondOrderCompleted = true;
                            npcController.currentFlowchart.SetIntegerVariable("StoryState", 2);
                            npcController.currentFlowchart.ExecuteBlock("Bayu1ThirdOrder");
                        }
                        break;
                }
            }
            else if (secondOrderCompleted == true && thirdOrderCompleted == false)
            {
                switch (npcOrderData.npcName)
                {
                    case "BuMaya":
                        Debug.Log("This is a unique NPC order for BuMaya.");
                        break;
                    case "PakAgus":
                        Debug.Log("This is a unique NPC order for PakAgus.");
                        break;
                    case "Bayu":
                        if (plasticBag.kopiKapal == thirdOrderData.quantity && thirdOrderCompleted == false && secondOrderCompleted == true)
                        {
                            Debug.Log("Bayu's third order completed.");
                            plasticBag.ResetPlastic();
                            thirdOrderCompleted = true;
                            npcController.currentFlowchart.SetIntegerVariable("StoryState", 3);
                            npcController.currentFlowchart.ExecuteBlock("Bayu1End");
                        }
                        break;
                }
            }

        }

    }
}
