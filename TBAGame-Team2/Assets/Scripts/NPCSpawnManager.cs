using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCSpawnManager : MonoBehaviour
{
    public static NPCSpawnManager instance;
    [SerializeField] GameObject npcPrefab;
    [SerializeField] Transform spawnPos;

    GameObject currentNPC;

    
    // Start is called before the first frame update

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
        var controller = currentNPC.GetComponent<npcController>();
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
        SpawnNPC();
    }
}
