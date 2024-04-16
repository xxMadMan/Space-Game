using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColourSphere : Interactable
{
    private new Renderer renderer;

    private void Awake()
    {
        renderer = GetComponent<Renderer>();
    }

    protected override void OnInteract()
    {
        //Debug.Log("CHANGING CORROR");

        Color color = new Color(
        Random.value,
        Random.value,
        Random.value,
        1f
        );
        
        renderer.material.SetColor("_Color", color);
    }
}
