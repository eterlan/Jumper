using UnityEditor;
using UnityEngine;

namespace Editor
{
    public class LimitFrameRate
    {
        [InitializeOnEnterPlayMode]
        public static void SetFrameRate()
        {
            Debug.Log("VAR");
            Application.targetFrameRate = 60;
        }
    }
}
