using System;
using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Test
{
    public class TestUniTask : MonoBehaviour
    {
        private CancellationTokenSource m_cts = new();

        [Button]
        public async UniTask Test1()
        {
            m_cts = new CancellationTokenSource();
            var isCancelled = await UniTask.Delay(TimeSpan.FromSeconds(1), cancellationToken: m_cts.Token).SuppressCancellationThrow();
            if (isCancelled)
            {
                Debug.Log("isCancelled");
                return;
            }
            Debug.Log("VAR");
        }

        [Button]
        public void Test2()
        {
            m_cts = new CancellationTokenSource();
            UniTask.Void(T, m_cts.Token);
        }

        [Button]
        public async void Test3()
        {
            var elapsedFrame = 0;
            while (elapsedFrame < 60)
            {
                elapsedFrame++;
                await UniTask.NextFrame();
            }

            Debug.Log("elapsed 60 frames");
        }
        
        public async UniTaskVoid FireAForget()
        {
            Debug.Log("Fire");
            await UniTask.Yield();
        }

        [Button]
        public void TT()
        {
            FireAForget().Forget();
            Debug.Log("AfterFire");
        }
        
        [Button]
        public void Cancel()
        {
            m_cts.Cancel();
            m_cts.Dispose();
        }

        async UniTaskVoid T(CancellationToken cancellationToken)
        {
            // await UniTask.Delay(TimeSpan.FromSeconds(1), cancellationToken: cancellationToken).SuppressCancellationThrow();
            var isCanceled = await UniTask.Delay(TimeSpan.FromSeconds(1), cancellationToken: cancellationToken).SuppressCancellationThrow();
            if (isCanceled)
            {
                Debug.Log("Canceled");
                return;
            }
            Debug.Log("VAR");
        }

        private void OnDisable()
        {
            Debug.Log("Disable");
            Cancel();
        }

        private void OnDestroy()
        {
            Debug.Log("Destroy");
        }
    }
}