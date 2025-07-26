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
        else if (currentNPCController.isOrderCompleted)
        {
            Destroy(currentNPC);
            currentNPC = null;
            currentNPCController = null;
        }
    }

    [Button("Add NPC")]
    void AddNPC()
    {
        currentNPC = Instantiate(npcPrefab, spawnPoint.position, spawnPoint.rotation);
        currentNPCController = currentNPC.GetComponent<NPCController>();
        currentNPCController.cameraManager = cameraManager;
    }
}
