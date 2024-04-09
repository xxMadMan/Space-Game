using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Outline))]
public abstract class Interactable : MonoBehaviour
{
    public abstract void InteractObject();

    public void ToggleOutline(){
        var outlineOBJ = GetComponent<Outline>();
        if (outlineOBJ.enabled)
            return;
            outlineOBJ.enabled = true;
            outlineOBJ.StartCoroutine(outlineOBJ.OutLineTimer());
    }
}
