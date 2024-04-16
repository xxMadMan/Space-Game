using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableScript : MonoBehaviour
{
    public MonoBehaviour script;

    public float delay = 1f;
    private float timer;
    
    public enum CompletionBehaviour
    {
        None,
        Disable,
        Destroy
    };

    public CompletionBehaviour completionBehaviour = CompletionBehaviour.None;

    private void OnEnable()
    {
        ResetTimer();
    }

    public void ResetTimer()
    {
        timer = 0f;
    }

    private void Update()
    {
        if (timer >= delay)
        {
            script.enabled = false;

            DoCompletionBehaviour();
            
            return;
        }

        timer += Time.deltaTime;
    }

    private void DoCompletionBehaviour()
    {
        switch(completionBehaviour)
        {
            case CompletionBehaviour.None: return;
            case CompletionBehaviour.Disable: enabled = false; break;
            case CompletionBehaviour.Destroy: DestroyImmediate(this); break;
        }
    }
}
