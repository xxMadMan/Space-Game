using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorAnimation : ButtonActivation
{
    public Animator doorAnimation;
    
    protected override void InteractButton()
    {
        doorAnimation = doorAnimation.GetComponent<Animator>();
        doorAnimation.SetBool("isOpen", true);
    }
}
