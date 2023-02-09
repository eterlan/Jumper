using System.Collections.Generic;
using System.Diagnostics;
using Sirenix.OdinInspector;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace ECS
{
    public class TestECS : MonoBehaviour
    {
        public int count;
        [Button]
        public void Test()
        {
            var a   = Stopwatch.StartNew();
            var       ecs = Ecs.instance;
            for (var i = 0; i < count; i++)
            {
                var id = i;
                ecs.TryAddComponent(id, new AComponent { a = i });
                var createB = Random.Range(0, 10) < 2;
                if (createB)
                {
                    ecs.TryAddComponent(id, new BComponent { a = i * 0.1f });
                }
            }

            Debug.Log($"{a.Elapsed.Milliseconds}ms");
            Ecs.instance.Foreach<AComponent, BComponent>((a, b) =>
            {
                Debug.Log("----- 2 -----");
                Debug.Log(a.a);
                Debug.Log(b.a);
            });
        }
        

        public struct AComponent : IComponentData
        {
            public int a;
        }

        public struct BComponent : IComponentData
        {
            public float a;
        }
    }
}