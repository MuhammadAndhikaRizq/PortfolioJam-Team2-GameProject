using UnityEngine;



[CreateAssetMenu(fileName = "NewItem", menuName = "Game/NpC Order Data")]
public class NPCOrderData : ScriptableObject
{
    public string blockName;
    public bool isUnique;
    public GameObject myFlowchart;
    public ItemData requestedItem;
}
