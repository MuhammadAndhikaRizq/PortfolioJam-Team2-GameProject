using UnityEngine;



[CreateAssetMenu(fileName = "NewItem", menuName = "Game/NpC Order Data")]
public class NPCOrderData : ScriptableObject
{
    public string orderName;
    public bool isUnique;
    public ItemData requestedItem;
}
