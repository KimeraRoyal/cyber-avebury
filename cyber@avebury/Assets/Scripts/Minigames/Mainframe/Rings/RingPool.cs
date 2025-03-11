using UnityEngine;
using UnityEngine.Pool;

namespace CyberAvebury.Minigames.Mainframe.Rings
{
    public class RingPool : MonoBehaviour
    {
        private IObjectPool<Ring> m_pool;

        [SerializeField] private Ring m_ringPrefab;

        private void Awake()
        {
            m_pool = new ObjectPool<Ring>(Create, Take, Return, Destroy, true, 10, 100);
        }

        public Ring Get()
            => m_pool.Get();

        public void Release(Ring _ring)
            => m_pool.Release(_ring);

        private Ring Create()
        {
            var ring = Instantiate(m_ringPrefab, transform);
            return ring;
        }

        private void Destroy(Ring _ring)
        {
            Destroy(_ring.gameObject);
        }

        private void Take(Ring _ring)
        {
            _ring.gameObject.SetActive(true);
            _ring.Activate();
        }

        private void Return(Ring _ring)
        {
            _ring.gameObject.SetActive(false);
        }
    }
}