using UnityEngine;

public class PlayerInteraction
{
    private PickUpController PickUpController;
    private Player Player;
    private PlayerMovement PlayerMovement;

    public void Initialize(Player _Player, PickUpController _PickUpController, PlayerMovement _PlayerMovement)
    {
        Player = _Player;
        PickUpController = _PickUpController;
        PlayerMovement = _PlayerMovement;
    }

    public void HandleInteractions()
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

        Camera cam = Player.playerCamera;

        if (Physics.Raycast(cam.transform.position, cam.transform.TransformDirection(Vector3.forward), out hit, Player.pickupDistance))
        {
            Interactable _interactable = hit.transform.GetComponentInParent<Interactable>();
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

                    if (_interactable.GetComponent<Ladder>())
                    {
                        PlayerMovement.ToggleLadderMovement(_interactable.GetComponent<Ladder>());
                    }
                }

                //I tried to make this the throw key however because the ignore raycast is a layer, 
                //we detect objects with layers. may need to be a tag Im not sure how to fix this without completely fucking shit up
                if (Input.GetKeyDown(InputMappings.Throw)){
                    if (_interactable.GetComponent<ComponentLocking>()){
                        _interactable.GetComponent<ComponentLocking>().PlaceComponent();
                    }
                }
            }
        }
    }
}
