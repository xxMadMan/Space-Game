using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLook : MonoBehaviour
{
    public float smoothSpeed = 0.125f;
    public float damping = 8f;
    public Transform cameraPosDestination;
    public Transform screenPosition;

    public void PlayerToScreenMovement() {
        Vector3 desiredPosition = cameraPosDestination.position;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;

        Quaternion rotation = Quaternion.LookRotation(screenPosition.position - transform.position);
        transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * damping);
    }

    public void ScreenBackToPlayer(Transform playerCamOrigin){

        //Debug.Log("RETURNING TO PLAYER");
        Vector3 returnPosition = playerCamOrigin.position;
        Vector3 smoothedReturn = Vector3.Lerp(transform.position, returnPosition, smoothSpeed);
        transform.position = smoothedReturn;

        Quaternion rotation = Quaternion.LookRotation(screenPosition.position - transform.position);
        transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * damping);
    }
}
