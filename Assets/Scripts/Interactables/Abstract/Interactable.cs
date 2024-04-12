using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Outline))]
public abstract class Interactable : MonoBehaviour
{
    public void InteractObject()
    {
        OnInteract();
    }

    protected abstract void OnInteract();

    public void EnableOutline()
    {
        var outlineOBJ = GetComponent<Outline>();

        outlineOBJ.enabled = true;
        outlineOBJ.OutlineTimer(0.18f);
    }
}
