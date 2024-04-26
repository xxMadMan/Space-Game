using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpController
{
    private Player Player;
    private Transform transform;

    //if you copy from below this point, you are legally required to like the video
    //private float rotationSensitivity = 1f; //how fast/slow the object is rotated in relation to mouse movement
    private PickUp heldPickup; //the pickup we are holding

    private Rigidbody heldObjRb; //rigidbody of object we pick up

    public bool holdingObj { get; private set; } = false;

    public bool canDrop { get; private set; } = true; //this is needed so we don't throw/drop object when rotating the object
    private bool closeInspect = false;

    private float closeInspectFactor = 0f;

    public void ToggleCloseInspect()
    {
        if (holdingObj && !heldPickup.closeInspectionEnabled)
        {
            closeInspect = false;
        }
        else
            closeInspect = !closeInspect;

        if (closeInspect)
        {
            closeInspectFactor = 0f;
        }
    }

    private int heldObjPreviousLayer;

    public void Initialize(Player _Player)
    {
        Player = _Player;
        transform = Player.transform;
    }

    public void HandlePickups()
    {
        if (heldPickup != null) //if player is holding object
        {
            MoveObject(); //keep object position at holdPos

            if (closeInspect && closeInspectFactor < 1f)
                closeInspectFactor = Mathf.Min(closeInspectFactor + Time.deltaTime * 4f, 1f);
            else if (!closeInspect && closeInspectFactor > 0f)
                closeInspectFactor = Mathf.Max(0f, closeInspectFactor - Time.deltaTime * 4f);
        }
    }

    public void PickUpObject(GameObject pickUpObj)
    {
        heldPickup = pickUpObj.GetComponent<PickUp>(); //assign heldObj to the object that was hit by the raycast (no longer == null)
        heldObjRb = pickUpObj.GetComponent<Rigidbody>(); //assign Rigidbody
        heldObjRb.isKinematic = true;
        heldObjPreviousLayer = pickUpObj.layer;
        pickUpObj.layer = CollisionLayers.HoldLayer; //change the object layer to the holdLayer
        //make sure object doesnt collide with player, it can cause weird bugs
        Physics.IgnoreCollision(heldPickup.GetComponent<Collider>(), Player.GetComponent<Collider>(), true);

        MoveObject();

        holdingObj = true;
    }

    public void ThrowObject()
    {
        DropObject(Player.playerCamera.transform.forward * Player.throwForce);
    }

    public void DropObject(Vector3 force = new Vector3())
    {
        closeInspectFactor = 0f;
        closeInspect = false;
        
        MoveObject();
        StopClipping(); //prevents object from clipping through walls

        //re-enable collision with player
        Physics.IgnoreCollision(heldPickup.GetComponent<Collider>(), Player.GetComponent<Collider>(), false);
        
        heldPickup.gameObject.layer = heldObjPreviousLayer; //object assigned back to default layer
        heldObjPreviousLayer = 0;
        heldPickup = null; //undefine game object
        
        heldObjRb.isKinematic = false;
        heldObjRb.AddForce(force, ForceMode.Impulse);
        heldObjRb = null; //undefine game object's rigidbody

        holdingObj = false;
    }

    void MoveObject()
    {
        Transform holdPos = Player.holdPos;
        
        heldPickup.transform.SetPositionAndRotation(
            holdPos.position + (Player.playerCamera.transform.position - holdPos.position).normalized * 0.5f * closeInspectFactor,
            holdPos.rotation * Quaternion.Euler(90f, 180f, 0f));
    }

    void StopClipping() //function only called when dropping/throwing
    {
        var clipRange = Vector3.Distance(heldPickup.transform.position, transform.position); //distance from holdPos to the camera
        //have to use RaycastAll as object blocks raycast in center screen
        //RaycastAll returns array of all colliders hit within the cliprange
        RaycastHit[] hits;
        hits = Physics.RaycastAll(transform.position, transform.TransformDirection(Vector3.forward), clipRange);
        //if the array length is greater than 1, meaning it has hit more than just the object we are carrying
        if (hits.Length > 1)
        {
            //change object position to camera position 
            heldPickup.transform.position = transform.position + new Vector3(0f, -0.5f, 0f); //offset slightly downward to stop object dropping above player 
            //if your player is small, change the -0.5f to a smaller number (in magnitude) ie: -0.1f
        }
    }
}
