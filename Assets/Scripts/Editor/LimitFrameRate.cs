using UnityEditor;
using UnityEngine;

namespace Editor
{
    public class LimitFrameRate
    {
        [InitializeOnEnterPlayMode]
        public static void SetFrameRate()
        {
            Application.targetFrameRate = 60;
        }
    }
}
