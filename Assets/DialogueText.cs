using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueText : MonoBehaviour
{
    public string dialogueText;
    public float duration = 2f;
    
    public void QueueDialogue()
    {
        GameplayUI.QueueDialogue(dialogueText, duration);
    }

    public void SkipDialogue()
    {
        GameplayUI.SkipDialogue();
    }
}
