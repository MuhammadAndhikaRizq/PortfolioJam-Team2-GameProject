using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewItem", menuName = "Game/NPC Data")]
public class NPCData : ScriptableObject
{
    public string npcName;
    public float maxPatienceBarValue;
}
