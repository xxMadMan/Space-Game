using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Computer : Interactable
{
    public Transform lookPosition;

    private GameObject playerObj;

    protected override void OnInteract()
    {
        playerObj = player.gameObject;
        playerObj.SetActive(false);

        player.camLook.LookAtScreen(transform.position, lookPosition.position);
    }

    void Update(){
        if (playerObj != null && playerObj.activeInHierarchy == false)
        {
            if (Input.GetKeyDown(InputMappings.Throw))
            {
                playerObj.SetActive(true);
                player.camLook.ReturnToPlayer();
            }
        }
    }

    IEnumerator PlayerEnable() {
        //Debug.Log("Waiting For Default");
        yield return null; // new WaitUntil();
        StopAllCoroutines();
    }
}
