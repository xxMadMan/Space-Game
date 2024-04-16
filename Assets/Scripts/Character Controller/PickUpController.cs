using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpController : MonoBehaviour
{
    public float throwForce = 500f; //force at which the object is thrown at
    public float pickUpRange = 5f; //how far the player can pickup the object from

    public GameObject _interactableObjects;
    public GameObject player;

    public Transform holdPos;

    public Camera cam;

    //if you copy from below this point, you are legally required to like the video
    //private float rotationSensitivity = 1f; //how fast/slow the object is rotated in relation to mouse movement
    private PickUp heldPickUp; //the pickup we are holding

    private Rigidbody heldObjRb; //rigidbody of object we pick up

    public bool holdingObj = false;

    public bool canDrop { get; private set; } = true; //this is needed so we don't throw/drop object when rotating the object
    private bool closeInspect = false;

    private float closeInspectFactor = 0f;

    public void ToggleCloseInspect()
    {
        if (holdingObj && !heldPickUp.closeInspectionEnabled)
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

    

    //Reference to script which includes mouse movement of player (looking around)
    //we want to disable the player looking around when rotating the object
    //example below 
    //MouseLookScript mouseLookScript;
    void Start()
    {
        //mouseLookScript = player.GetComponent<MouseLookScript>();
    }
    void Update()
    {
        if (heldPickUp != null) //if player is holding object
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
        heldPickUp = pickUpObj.GetComponent<PickUp>(); //assign heldObj to the object that was hit by the raycast (no longer == null)
        heldObjRb = pickUpObj.GetComponent<Rigidbody>(); //assign Rigidbody
        heldObjRb.isKinematic = true;
        heldObjPreviousLayer = pickUpObj.layer;
        pickUpObj.layer = CollisionLayers.HoldLayer; //change the object layer to the holdLayer
        //make sure object doesnt collide with player, it can cause weird bugs
        Physics.IgnoreCollision(heldPickUp.GetComponent<Collider>(), player.GetComponent<Collider>(), true);

        MoveObject();

        holdingObj = true;
    }
    public void DropObject(Vector3 force = new Vector3())
    {
        closeInspectFactor = 0f;
        closeInspect = false;
        
        MoveObject();
        StopClipping(); //prevents object from clipping through walls

        //re-enable collision with player
        Physics.IgnoreCollision(heldPickUp.GetComponent<Collider>(), player.GetComponent<Collider>(), false);
        
        heldPickUp.gameObject.layer = heldObjPreviousLayer; //object assigned back to default layer
        heldObjPreviousLayer = 0;
        heldPickUp = null; //undefine game object
        
        heldObjRb.isKinematic = false;
        heldObjRb.AddForce(force);
        heldObjRb = null; //undefine game object's rigidbody

        holdingObj = false;
    }
    public void ThrowObject()
    {
        DropObject(cam.transform.forward * throwForce);
    }
    void MoveObject()
    {
        heldPickUp.transform.position = holdPos.transform.position + (Camera.main.transform.position - holdPos.transform.position).normalized * 0.5f * closeInspectFactor;
        heldPickUp.transform.rotation = holdPos.transform.rotation * Quaternion.Euler(90f, 180f, 0f);
    }

    void StopClipping() //function only called when dropping/throwing
    {
        var clipRange = Vector3.Distance(heldPickUp.transform.position, transform.position); //distance from holdPos to the camera
        //have to use RaycastAll as object blocks raycast in center screen
        //RaycastAll returns array of all colliders hit within the cliprange
        RaycastHit[] hits;
        hits = Physics.RaycastAll(transform.position, transform.TransformDirection(Vector3.forward), clipRange);
        //if the array length is greater than 1, meaning it has hit more than just the object we are carrying
        if (hits.Length > 1)
        {
            //change object position to camera position 
            heldPickUp.transform.position = transform.position + new Vector3(0f, -0.5f, 0f); //offset slightly downward to stop object dropping above player 
            //if your player is small, change the -0.5f to a smaller number (in magnitude) ie: -0.1f
        }
    }
}
