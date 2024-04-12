using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Interaction : MonoBehaviour
{
    public Camera cam;

    public float hitRange = 3f;

    private PickUpController PickUpController;

    private void Awake()
    {
        PickUpController = GetComponent<PickUpController>();
    }

    void Update()
    {
        if (!PickUpController.holdingObj)
        {
            CheckForInteractable();
        }
        else
        {
            HandlePickupControllerInputs(); //do this here to ensure order of execution of these functions
        }
    }

    private void HandlePickupControllerInputs()
    {
        if (PickUpController.canDrop)
        {
            if (Input.GetKeyDown(InputMappings.Interact))
            {
                PickUpController.DropObject();
            }
            else if (Input.GetKeyDown(InputMappings.Throw))
            {
                PickUpController.ThrowObject();
            }
        }

        if (!PickUpController.holdingObj) //no need to continue if not holding object anymore
            return;

        if (Input.GetKeyDown(InputMappings.CloseInspect))
        {
            PickUpController.ToggleCloseInspect();
        }
    }

    private void CheckForInteractable()
    {
        //perform raycast to check if player is looking at object within pickuprange
        RaycastHit hit;

        if (Physics.Raycast(cam.transform.position, cam.transform.TransformDirection(Vector3.forward), out hit, hitRange))
        {
            Interactable _interactable = hit.transform.GetComponent<Interactable>();
            //Debug.Log("Raycast hit");

            if (_interactable)
            {
                _interactable.EnableOutline();

                if (Input.GetKeyDown(InputMappings.Interact))
                {
                    if (_interactable.GetComponent<Rigidbody>())
                    {
                        PickUpController.PickUpObject(_interactable.gameObject);
                    }

                    _interactable.InteractObject();
                }
            }
        }
    }
}
