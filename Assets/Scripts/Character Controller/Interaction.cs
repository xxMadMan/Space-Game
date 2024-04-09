using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Interaction : MonoBehaviour
{

    public Camera cam;

    public float hitRange = 3f;

    void Update()
    {

        //perform raycast to check if player is looking at object within pickuprange
        RaycastHit hit;
        if (Physics.Raycast(cam.transform.position, cam.transform.TransformDirection(Vector3.forward), out hit, hitRange))
        {
            Interactable _interactable = hit.transform.GetComponent<Interactable>();
            Debug.Log("Raycast hit");
            if (_interactable){
                if (Input.GetKeyDown(KeyCode.Mouse0)) //change E to whichever key you want to press to pick up
                {

                    if (_interactable)
                    {
                        _interactable.InteractObject();
                    }
                }
                Debug.Log("Toggling Outline");
                _interactable.ToggleOutline();
            } 
        }
    }
}
