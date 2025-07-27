using UnityEngine;



[CreateAssetMenu(fileName = "NewItem", menuName = "Game/NPC Order Data")]
public class NPCOrderData : ScriptableObject
{
    public string npcName;
    public string blockName;
    public bool isUnique;
    public string myFlowchart;
    public int quantity;
    public ItemData requestedItem;
}
