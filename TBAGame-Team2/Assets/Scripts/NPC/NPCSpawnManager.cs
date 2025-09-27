using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using Sirenix.OdinInspector;

public class NPCSpawnManager : MonoBehaviour
{
    public static NPCSpawnManager instance;
    [SerializeField] List<GameObject> npcPrefabs = new();
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
        int randomIndex = Random.Range(0, npcPrefabs.Count);
        currentNPC = Instantiate(npcPrefabs[randomIndex], spawnPos.position, Quaternion.identity);
        currentNPC.transform.SetParent(this.transform.parent);
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
        if (Cashier.Instance != null) Cashier.Instance.ResetMiniGame();
        SpawnNPC();
    }
}
