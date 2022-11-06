using UnityEngine;

public class ColorUtil
{
    public static Color pink = new Color(0.925f, 0.746f, 0.746f, 1f);
    public static Color yellow = new Color(0.925f, 0.873f, 0.597f, 1f);
    public static Color blue = new Color(0.549f, 0.788f, 0.849f, 1f);
    public static Color green = new Color(0.644f, 0.821f, 0.608f, 1f);
    public static Color red = new Color(0.858f, 0.328f, 0.328f, 1f);
    public static Color purple = new Color(0.732f, 0.580f, 1f, 1f);
    public static Color orange = new Color(0.991f, 0.457f, 0.341f, 1f);
    public static Color cyan = new Color(0.480f, 0.802f, 0.794f, 1f);
    public static readonly Color[] Colors = new []{pink, yellow, blue, green, red, purple, orange, cyan};
    public static int lastIndex;
    public static Color GetColor()
    {
        var color = Colors[lastIndex++];
        if (lastIndex >= Colors.Length - 1)
        {
            lastIndex = 0;
        }
        return color;
    }

    public static Color GetRandomColor()
    {
        var index = Random.Range(0, Colors.Length);
        return Colors[index];
    }

    public static Color Close = new Color(0.675f, 0.439f, 0.533f, 1f);
    public static Color Open = new Color(0.925f, 0.8f, 0.698f, 1f);
    // 暖紫
    // public static class Close
    // {
    //     public const float R = 0.675f;
    //     public const float G = 0.439f;
    //     public const float B = 0.533f;
    // }

    // 淡黄
    // public static class Open
    // {
    //     public const float R = 0.925f;
    //     public const float G = 0.8f;
    //     public const float B = 0.698f;
    // }
}