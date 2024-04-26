using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class ExponentialTest : MonoBehaviour
{
    [Range(0, 1)]
    public float value = 0f;

    [Range(0, 1)]
    public float powTwo;

    [Range(0, 1)]
    public float exponential;

    [Range(0, 1)]
    public float idk;

    void Update()
    {
        powTwo = value * value;
        exponential = 1f - Mathf.Pow(1f - value, 2);
        idk = IDK(value);
    }

    private float IDK(float value)
    {
        float z1z = Mathf.Pow(1.0f - Mathf.Abs(value * 2f - 1f), 2f);

        if (value < 0.5f)
        {
            return z1z * 0.5f;
        }
        else
        {
            return 0.5f + (1f - z1z) / 2f;
        }
    }
}
