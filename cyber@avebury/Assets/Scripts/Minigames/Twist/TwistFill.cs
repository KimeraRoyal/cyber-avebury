using UnityEngine;

namespace CyberAvebury
{
    [RequireComponent(typeof(RadialFill))]
    public class TwistFill : MonoBehaviour
    {
        private TwistRing m_ring;

        private RadialFill m_fill;

        private void Awake()
        {
            m_ring = GetComponentInParent<TwistRing>();

            m_fill = GetComponent<RadialFill>();
        }

        private void Update()
        {
            var arcPoint = m_ring.DegreeRange / 2;
            m_fill.ArcPoint1 = arcPoint;
            m_fill.ArcPoint2 = arcPoint;
        }
    }
}
