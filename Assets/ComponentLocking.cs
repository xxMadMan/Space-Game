using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComponentLocking : Interactable
{

    public GameObject lockingPoint;
    GameObject componentObject;

    protected override void OnInteract(){
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Component")
        {
            componentObject = other.gameObject;
        }
    }

    void OnTriggerExit(Collider other){

        if (other.gameObject.tag == "Component"){
            componentObject = null;
        }
    }

    //The ray that casts from the player will call this

    bool isPlaced = false;

    //I hate everything about this, like no shit it pisses me off that this works
    //unless you place the object on the pad and then click the bottom box really fast. then it doesnt work so well.

    public void PlaceComponent(){
        if (componentObject && isPlaced == false){
            componentObject.tag = "Untagged";
            componentObject.GetComponent<BoxCollider>().enabled = false;
            componentObject.transform.position = lockingPoint.transform.position;
            componentObject.transform.localRotation = lockingPoint.transform.localRotation;
            componentObject.GetComponent<Rigidbody>().isKinematic = true;
            componentObject.transform.localScale = componentObject.transform.localScale * 0.75f;
            componentObject.transform.parent = this.gameObject.transform;
            isPlaced = true;
        } else if (componentObject && isPlaced == true) {
            componentObject.tag = "Component";
            componentObject.transform.position = lockingPoint.transform.position + new Vector3(0,0.1f,0);
            componentObject.GetComponent<BoxCollider>().enabled = true;
            componentObject.GetComponent<Rigidbody>().isKinematic = false;
            componentObject.transform.localScale = componentObject.transform.localScale * 1.5f;
            componentObject.transform.parent = null;
            isPlaced = false;

        }
    }


}
