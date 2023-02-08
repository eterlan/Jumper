using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ECS
{
    public class TestECS : MonoBehaviour
    {
        [Button]
        public void Test()
        {
            var entity1 = 100;
            var entity2 = 200;

            Ecs.instance.TryAddComponent(entity1, new AComponent { a = "AB" });
            Ecs.instance.TryAddComponent(entity1, new BComponent { a = "B" });
            Ecs.instance.TryAddComponent(entity2, new AComponent { a = "C" });

            Ecs.instance.Foreach((AComponent a) =>
            {
                Debug.Log("----- 1 -----");
                Debug.Log(a.a);
            });

            Ecs.instance.Foreach((AComponent a, BComponent b) =>
            {
                Debug.Log("----- 2 -----");
                Debug.Log(a.a);
            });
        }
        

        public struct AComponent : IComponentData
        {
            public string a;
        }

        public struct BComponent : IComponentData
        {
            public string a;
        }
    }
}