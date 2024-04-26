using UnityEngine;

public static class MathLibrary
{
    public static float SlowFastSlow(float value01)
    {
        value01 = Mathf.Clamp01(value01);
        
        float z1z = Mathf.Pow(1.0f - Mathf.Abs(value01 * 2f - 1f), 2f);

        if (value01 < 0.5f)
        {
            return z1z * 0.5f;
        }
        else
        {
            return 0.5f + (1f - z1z) / 2f;
        }
    }
}
