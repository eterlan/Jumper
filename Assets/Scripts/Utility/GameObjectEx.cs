using UnityEngine;

namespace Utility
{
    public static class GameObjectEx
    {
        public static T GetOrAddComponent<T>(GameObject go) where T : Component
        {
            if (go.TryGetComponent<T>(out var c))
            {
                return c;
            }

            return go.AddComponent<T>();
        }
    }
}