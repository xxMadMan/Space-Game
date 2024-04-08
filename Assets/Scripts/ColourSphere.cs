using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColourSphere : Interactable
{
    

    public override void InteractObject()
    {
        //Debug.Log("CHANGING CORROR");

        Color color = new Color(
        Random.value,
        Random.value,
        Random.value,
        1f
        );
        GetComponent<Renderer>().material.SetColor("_Color", color);
    }
}
