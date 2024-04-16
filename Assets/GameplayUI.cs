using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEngine;
using Unity.VisualScripting;

public class GameplayUI : MonoBehaviour
{

    static GameplayUI sceneInstance;

    public TextMeshProUGUI dialogueBox;

    void Awake()
    {
        sceneInstance = this;    
    }

    public static void SetDialogue(string setTo){
        sceneInstance.dialogueBox.text = setTo;
    }

}
