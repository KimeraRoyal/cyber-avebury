using UnityEngine;

namespace CyberAvebury
{
    [RequireComponent(typeof(Animator))]
    public class BossAnimator : MonoBehaviour
    {
        private static readonly int s_hurt = Animator.StringToHash("Hurt");
        private static readonly int s_defeated = Animator.StringToHash("Defeated");
        
        private Boss m_boss;
        
        private Animator m_animator;

        private void Awake()
        {
            m_boss = GetComponentInParent<Boss>();
            
            m_animator = GetComponent<Animator>();
            
            m_boss.OnHurt.AddListener(OnHurt);
            m_boss.OnDefeated.AddListener(OnDefeated);
        }

        private void OnHurt()
        {
            m_animator.SetTrigger(s_hurt);
        }

        private void OnDefeated()
        {
            m_animator.SetBool(s_defeated, true);
        }
    }
}
