using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class CameraLook : MonoBehaviour
{
    public Player player;

    private PositionConstraint playerPosConstraint;
    private RotationConstraint playerRotConstraint;

    private Vector3 startPosition;
    private Quaternion startRotation;

    public float smoothDuration = 0.4f;

    private void Awake()
    {
        playerPosConstraint = GetComponent<PositionConstraint>();
        playerRotConstraint = GetComponent<RotationConstraint>();
    }

    public void LookAtScreen(Vector3 lookAt, Vector3 lookFrom)
    {
        StorePositionAndRotation();

        StopAllCoroutines();
        StartCoroutine(PlayerToScreen(lookAt, lookFrom));
    }

    private IEnumerator PlayerToScreen(Vector3 lookAt, Vector3 lookFrom)
    {
        //Debug.Log("LOOKING AT SCREEN");
        
        playerPosConstraint.constraintActive = false;
        playerRotConstraint.constraintActive = false;

        float timer = 0f;

        while (timer < smoothDuration)
        {
            transform.position = Vector3.Lerp(startPosition, lookFrom, MathLibrary.SlowFastSlow(timer / smoothDuration));
            transform.rotation = Quaternion.Lerp(startRotation, Quaternion.LookRotation(lookAt - transform.position), MathLibrary.SlowFastSlow(timer / smoothDuration));

            timer += Time.deltaTime;

            yield return null;
        }
    }

    public void ReturnToPlayer()
    {
        StorePositionAndRotation();

        StopAllCoroutines();
        StartCoroutine(ScreenBackToPlayer());
    }

    private IEnumerator ScreenBackToPlayer()
    {
        //Debug.Log("RETURNING TO PLAYER");

        float timer = 0f;

        while (timer < smoothDuration)
        {
            transform.position = Vector3.Slerp(startPosition, player.camPosition.position, MathLibrary.SlowFastSlow(timer / smoothDuration));
            transform.rotation = Quaternion.Slerp(startRotation, player.camPosition.rotation, MathLibrary.SlowFastSlow(timer / smoothDuration));

            timer += Time.deltaTime;

            yield return null;
        }

        playerPosConstraint.constraintActive = true;
        playerRotConstraint.constraintActive = true;
    }

    private void StorePositionAndRotation()
    {
        startPosition = transform.position;
        startRotation = transform.rotation;
    }
}
