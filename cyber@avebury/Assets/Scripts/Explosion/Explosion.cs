using KR;
using UnityEngine;

namespace CyberAvebury
{
    [RequireComponent(typeof(FrameAnimator))]
    public class Explosion : MonoBehaviour
    {
        private ExplosionPool m_pool;
        
        private FrameAnimator m_animator;
        
        private void Awake()
        {
            m_pool = GetComponentInParent<ExplosionPool>();
            
            m_animator = GetComponent<FrameAnimator>();
            m_animator.OnFinished.AddListener(OnAnimationFinished);
        }

        private void OnEnable()
        {
            m_animator.ResetAnimation();
        }

        private void OnAnimationFinished()
        {
            m_pool.Release(this);
        }
    }
}
