using UnityEngine;

public static class MathHelper
{
    public static Vector3 Fill(this Vector3 input, float value)
    {
        input.x = value;
        input.y = value;
        input.z = value;
        return input;
    }

    public static float Remap(float x, float min1, float max1, float min2, float max2)
    {
        var clampX = ClampAuto(x, min1, max1);
        var result = (clampX - min1) / (max1 - min1) * (max2 - min2) + min2;
        return result;
    }

    /// <summary>
    /// 自动判断大小
    /// </summary>
    /// <returns></returns>
    public static float ClampAuto(float x, float a, float b)
    {
        var min = a;
        var max = b;
        if (a > b)
        {
            min = b;
            max = a;
        }

        return Mathf.Clamp(x, min, max);
    }
}