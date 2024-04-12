using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class PaperLocking : Interactable
{
    bool isLocked = false;
    
    public override void InteractObject()
    {
        isLocked = !isLocked;
        if (!isLocked)
            return;
        transform.localPosition = new Vector3();
        transform.localRotation = new Quaternion();
        transform.LookAt(Camera.main.transform.position);
        transform.rotation *= Quaternion.Euler(90,0,0);
    }
    //Bens list of shit to add feature this? XoXo Gossip grills
    // a click option that uses the canvas feature and covers most of the screen with paper/text
}
