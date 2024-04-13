using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Computer : Interactable
{
    GameObject playerObject;
    CameraLook camLook;
    int cameraState = 0;
    GameObject camPosition;
    public float waitSpeed;

    void Awake(){
        playerObject = GameObject.FindGameObjectWithTag("Player");
        camLook = Camera.main.transform.GetComponent<CameraLook>();
        camPosition = GameObject.FindWithTag("CurrentCamPosition");
    }

    protected override void OnInteract()
    {
        Camera.main.transform.parent = null;
        playerObject.SetActive(false);
        cameraState = 2;
    }

    void FixedUpdate(){
        switch(cameraState){
            default:
                //print("Default State");
                break;
            case 2:
                //moving from player to screen object
                camLook.PlayerToScreenMovement();
                //Debug.Log("Looking At Screeeen");
                break;
            case 1:
                //moving from screen back to player
                camLook.ScreenBackToPlayer(camPosition.transform);
                //Debug.Log("RETURNING TO PLAYER");
                break;
        }
    }

    void Update(){
        if (playerObject.activeInHierarchy == false)
        {
            if (Input.GetKeyDown(InputMappings.Throw)){
                playerObject.SetActive(true);
                Camera.main.transform.parent = playerObject.gameObject.transform;
                cameraState = 1;
                StartCoroutine(PlayerEnable());
            }
        }
    }

    IEnumerator PlayerEnable() {
        //Debug.Log("Waiting For Default");
        yield return new WaitForSeconds(waitSpeed);
        cameraState = 0;
        StopAllCoroutines();
    }
}
