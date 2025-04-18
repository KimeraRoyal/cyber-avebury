using DG.Tweening;
using UnityEngine;

namespace CyberAvebury
{
    [RequireComponent(typeof(RadialFill))]
    public class TwistFill : MonoBehaviour
    {
        private TwistRing m_ring;

        private RadialFill m_fill;

        [SerializeField] private float m_colorChangeDuration = 0.1f;

        private float m_arcPoint;

        private Tween m_arcPointTween;

        public float ArcPoint
        {
            get => m_arcPoint;
            set
            {
                if(Mathf.Abs(m_arcPoint - value) < 0.001f) { return; }
                m_arcPoint = value;
                m_fill.ArcPoint1 = m_arcPoint;
                m_fill.ArcPoint2 = m_arcPoint;
            }
        }

        private void Awake()
        {
            m_ring = GetComponentInParent<TwistRing>();
            m_ring.OnDegreeRangeChanged.AddListener(OnRingDegreeRangeChanged);
            m_ring.OnActiveChanged.AddListener(OnRingActiveChanged);

            m_fill = GetComponent<RadialFill>();
        }

        private void Start()
        {
            m_arcPoint = 180.0f;
            m_fill.ArcPoint1 = m_arcPoint;
            m_fill.ArcPoint2 = m_arcPoint;
        }

        private void OnRingDegreeRangeChanged(float _degreeRange)
        {
            if (!m_ring.IsActive) { return; }
            AnimateArcPoint(180.0f - _degreeRange / 2.0f);
        }

        private void OnRingActiveChanged(bool _isActive)
        {
            var range = _isActive ? m_ring.DegreeRange : 0.0f;
            AnimateArcPoint(180.0f - range / 2.0f);
        }

        private void AnimateArcPoint(float _value)
        {
            if(m_arcPointTween is { active: true }) { m_arcPointTween.Kill(); }
            m_arcPointTween = DOTween.To(() => ArcPoint, _value => ArcPoint = _value, _value, m_colorChangeDuration);
        }
    }
}
