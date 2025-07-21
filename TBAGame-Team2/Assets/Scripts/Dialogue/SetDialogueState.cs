using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;

[CommandInfo("Custom", "Set Dialogue State", "Sets the dialogue state")]
public class SetDialogueState : Command
{
    public bool dialogueActive;
    
    public override void OnEnter()
    {
        DialogueManager.isDialogueActive = dialogueActive;
        Continue();
    }
}
