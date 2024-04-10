using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ButtonActivation : Interactable
{
    
    protected abstract void InteractButton();

    public override void InteractObject()
    {
        InteractButton();
        Debug.Log("it works");
    }
}
