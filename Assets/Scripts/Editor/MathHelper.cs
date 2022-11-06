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
}