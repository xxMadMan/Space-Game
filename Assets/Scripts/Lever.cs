using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : MonoBehaviour
{
    float rotationPosition = 45f;

    bool rotation = false;

    public void RotateLever(){
        rotation = !rotation;
        if (rotation == true)
        {
            transform.Rotate(new Vector3(0, 0, rotationPosition *2));
            GameplayUI.SetDialogue(null);
        }
        else
        {
            transform.Rotate(new Vector3(0, 0, -rotationPosition * 2));

        }
    }

}
