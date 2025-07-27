using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class NPCManager : MonoBehaviour
{
    [LabelText("NPC Prefab")]
    public GameObject npcPrefab;
    [SerializeField] private Transform spawnPoint;
    [ReadOnly, ShowInInspector] private GameObject currentNPC;
    [ReadOnly, ShowInInspector] private NPCController currentNPCController;
    private CameraManager cameraManager;
    NPCOrderData orderData;

    // Start is called before the first frame update
    void Start()
    {
        cameraManager = FindObjectOfType<CameraManager>();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (currentNPC == null || currentNPCController == null)
        {
            AddNPC();
        }
        /*else if (currentNPCController.isOrderCompleted)
        {
            Destroy(currentNPC);
            currentNPC = null;
            currentNPCController = null;
        }*/
    }

    [Button("Add NPC")]
    void AddNPC()
    {
        currentNPC = Instantiate(npcPrefab, spawnPoint.position, spawnPoint.rotation);
        currentNPCController = currentNPC.GetComponent<NPCController>();
        currentNPCController.cameraManager = cameraManager;
        orderData = currentNPCController.currentOrder;
        if (orderData.isUnique == true)
        {
            switch (orderData.npcName)
            {
                case "BuMaya":
                    currentNPCController.currentSecondOrder = currentNPCController.secondOrder[0];
                    break;
                case "PakAgus":
                    Debug.Log("NPC PakAgus is created with unique order.");
                    break;
                case "Bayu":
                    Debug.Log("NPC Bayu is created with unique order.");
                    break;
                default:
                    Debug.LogWarning("Unknown NPC name for unique order.");
                    break;
            }
        }
        else
        {
            Debug.Log($"NPC {orderData.npcName} is created with a regular order.");
        }
    }
}
