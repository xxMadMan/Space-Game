using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingSpace : MonoBehaviour
{
    public float _speed = 0.2f;
    void Update()
    {
        transform.Rotate(0, _speed * Time.deltaTime, 0);
    }
}
