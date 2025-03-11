using UnityEngine;

namespace CyberAvebury.Minigames.Mainframe.Rings
{
    [RequireComponent(typeof(Ring))]
    public class RingPoolReturner : MonoBehaviour
    {
        private RingPool m_pool;

        private Ring m_ring;
        
        private void Awake()
        {
            m_pool = GetComponentInParent<RingPool>();

            m_ring = GetComponent<Ring>();
            m_ring.OnDeactivated.AddListener(OnRingDeactivated);
        }

        private void OnRingDeactivated()
        {
            m_pool.Release(m_ring);
        }
    }
}