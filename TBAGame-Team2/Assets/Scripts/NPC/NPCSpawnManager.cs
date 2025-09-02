using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using Sirenix.OdinInspector;

public class NPCSpawnManager : MonoBehaviour
{
    public static NPCSpawnManager instance;
    [SerializeField] GameObject npcPrefab;
    [SerializeField] Transform spawnPos;

    [ShowInInspector, Sirenix.OdinInspector.ReadOnly] GameObject currentNPC;

    void Awake()
    {
        instance = this;
    }
    void Start()
    {
        SpawnNPC();
    }

    public void SpawnNPC()
    {
        if (currentNPC != null) return;
        currentNPC = Instantiate(npcPrefab, spawnPos.position, Quaternion.identity);
        var controller = currentNPC.GetComponent<Customer>();
        controller.OnLeave += HandleNPCLeave;
    }

    void HandleNPCLeave()
    {
        StartCoroutine(OnNPCLeave());
    }

    IEnumerator OnNPCLeave()
    {
        yield return new WaitForSeconds(2f);
        currentNPC = null;
        Cashier.Instance.ResetMiniGame();
        SpawnNPC();
    }
}
