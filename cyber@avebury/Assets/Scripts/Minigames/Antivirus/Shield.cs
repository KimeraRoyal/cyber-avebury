using KR;
using UnityEngine;

namespace CyberAvebury
{
    [RequireComponent(typeof(RectTransform), typeof(FrameAnimator))]
    public class Shield : MonoBehaviour
    {
        private Antivirus m_antivirus;
        
        private RectTransform m_rect;
        private FrameAnimator m_animator;

        [SerializeField] private FrameAnimation m_fullAnimation;
        [SerializeField] private FrameAnimation m_brokenAnimation;

        private int m_index;
        private bool m_wasFull;

        public Antivirus Antivirus
        {
            get => m_antivirus;
            set => m_antivirus = value;
        }

        public Vector2 Position
        {
            get => m_rect.anchoredPosition;
            set => m_rect.anchoredPosition = value;
        }

        public int Index
        {
            get => m_index;
            set => m_index = value;
        }

        private void Awake()
        {
            m_rect = GetComponent<RectTransform>();
            m_animator = GetComponent<FrameAnimator>();
            m_wasFull = true;
        }

        private void Start()
        {
            m_antivirus.OnScoreUpdated.AddListener(OnScoreUpdated);
        }

        private void OnScoreUpdated(int _score)
        {
            var testIndex = m_antivirus.TargetScore - _score;
            var full = testIndex > m_index;
            
            if(full == m_wasFull) { return; }
            m_animator.Animation = full ? m_fullAnimation : m_brokenAnimation;
            m_wasFull = full;
        }
    }
}
