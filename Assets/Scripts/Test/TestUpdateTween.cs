using System;
using DG.Tweening;
using UnityEngine;

namespace Test
{
    // 尝试更新DOTween的目标位置, 看看是不是一样的结果
    // 看来确实是可行的, 但要注意的是到达目标位置之后会停下来, OnUpdate不再会被调用, 因此要自己在Update里面写
    public class TestUpdateTween : MonoBehaviour
    {
        private Camera    m_camera;
        public  Transform targetPos;
        public  float     duration  = 2;
        public  float     tolerance = 1;
        [SerializeField]
        private bool snapStartValue = false;
        [SerializeField]
        private float newDuration = -1;
        [SerializeField]
        private bool restart;

        private Tweener m_tween;

        private void Awake()
        {
            m_camera = Camera.main;
            m_tween  = transform.DOMove(targetPos.position, duration).SetAutoKill(false);
            m_tween.OnComplete(() =>
            {
                Debug.Log($"active: {m_tween.active}");
                Debug.Log($"complete: {m_tween.IsComplete()}");
                Debug.Log($"playing: {m_tween.IsPlaying()}");
            });
        }

        private void Update()
        {
            if (Vector3.Distance(transform.position, targetPos.position) > tolerance)
            {
                m_tween.ChangeEndValue(targetPos.position, newDuration, snapStartValue);
                if (!m_tween.IsPlaying())
                {
                    if (restart)
                    {
                        m_tween.Restart();
                    }
                    else
                    {
                        m_tween.Play();
                    }
                }
            }
        }
    }
}