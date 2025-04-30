using KR;
using UnityEngine;

namespace CyberAvebury
{
    [RequireComponent(typeof(FrameAnimator))]
    public class ObeliskShield : MonoBehaviour
    {
        private Obelisk m_obelisk;

        private FrameAnimator m_animator;

        [SerializeField] private FrameAnimation m_defaultAnimation;
        [SerializeField] private FrameAnimation m_beatenAnimation;
        
        private int m_index;

        public int Index
        {
            get => m_index;
            set => m_index = value;
        }

        private void Awake()
        {
            m_obelisk = GetComponentInParent<Obelisk>();

            m_animator = GetComponent<FrameAnimator>();
            
            m_obelisk.OnSubgamePassed.AddListener(OnSubgamePassed);
        }

        private void Start()
        {
            OnSubgamePassed(m_obelisk.CurrentSubgameIndex - 1);
        }

        private void OnSubgamePassed(int _subgameIndex)
        {
            m_animator.Animation = _subgameIndex >= m_index ? m_beatenAnimation : m_defaultAnimation;
        }
    }
}
