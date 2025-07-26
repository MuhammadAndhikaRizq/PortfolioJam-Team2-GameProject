using UnityEngine;



[CreateAssetMenu(fileName = "NewItem", menuName = "Game/NpC Order Data")]
public class NPCOrderData : ScriptableObject
{
    public string blockName;
    public bool isUnique;
    public string myFlowchart;
    public GameObject character;
    public GameObject playerCharacter;
    public ItemData requestedItem;
}
