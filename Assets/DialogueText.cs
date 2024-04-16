using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueText : MonoBehaviour
{
    public string dialogueText;
    
    public void DisplayDialogue(){
        GameplayUI.SetDialogue(dialogueText);
    }
}
