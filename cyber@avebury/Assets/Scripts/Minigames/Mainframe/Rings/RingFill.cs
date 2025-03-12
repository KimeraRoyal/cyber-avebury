using CyberAvebury.Minigames.Mainframe.Rings;
using UnityEngine;

namespace CyberAvebury
{
    [RequireComponent(typeof(RadialFill))]
    public class RingFill : MonoBehaviour
    {
        private Ring m_ring;

        private RadialFill m_fill;

        private void Awake()
        {
            m_ring = GetComponentInParent<Ring>();

            m_fill = GetComponent<RadialFill>();
            
            m_ring.OnLifetimeUpdated.AddListener(OnRingLifetimeUpdated);
        }

        private void OnRingLifetimeUpdated(float _lifetime)
        {
            var t = m_ring.Progress;
            
            m_fill.ArcPoint1 = t * 360.0f;
        }
    }
}
