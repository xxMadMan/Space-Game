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

    private List<IEnumerator> dialogueQueue = new();

    private bool displayingDialogue;
    private bool skipDialogue;

    private const float DialogueCooldown = 0.4f;

    void Awake()
    {
        sceneInstance = this;
    }

    private void Start()
    {
        dialogueBox.enabled = false;
    }

    private void Update()
    {
        if (!displayingDialogue && dialogueQueue.Count > 0)
        {
            StartCoroutine(dialogueQueue[0]);
        }
    }

    public static void QueueDialogue(string dialogue, float duration)
    {
        if (sceneInstance == null)
        {
            sceneInstance = FindObjectOfType<GameplayUI>();

            if (sceneInstance == null)
            {
                Debug.Log("GameplayUI not found in scene");

                return;
            }
        }

        sceneInstance.dialogueQueue.Add(sceneInstance.DisplayDialogue(dialogue, duration));
    }

    public static void SkipDialogue()
    {
        if (sceneInstance.dialogueQueue.Count > 0)
        {
            sceneInstance.skipDialogue = true;
        }
    }

    private IEnumerator DisplayDialogue(string dialogue, float duration)
    {
        displayingDialogue = true;

        sceneInstance.dialogueBox.text = dialogue;
        dialogueBox.enabled = true;

        float timer = duration;

        while (timer > 0 && !skipDialogue)
        {
            timer -= Time.deltaTime;

            yield return null;
        }

        if (!skipDialogue)
        {
            yield return new WaitForSeconds(DialogueCooldown);
        }

        dialogueQueue.RemoveAt(0);

        dialogueBox.enabled = false;
        displayingDialogue = false;
        skipDialogue = false;
    }
}
