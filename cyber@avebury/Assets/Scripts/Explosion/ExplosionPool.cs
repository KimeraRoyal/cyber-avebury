using UnityEngine;
using UnityEngine.Pool;

namespace CyberAvebury
{
    public class ExplosionPool : MonoBehaviour
    {
        private IObjectPool<Explosion> m_pool;

        [SerializeField] private Explosion m_explosionPrefab;

        private void Awake()
        {
            m_pool = new ObjectPool<Explosion>(Create, Take, Return, Destroy, true, 10, 100);
        }

        public Explosion Get()
            => m_pool.Get();

        public void Release(Explosion _explosion)
            => m_pool.Release(_explosion);

        private Explosion Create()
        {
            var explosion = Instantiate(m_explosionPrefab, transform);
            return explosion;
        }

        private void Destroy(Explosion _explosion)
        {
            Destroy(_explosion.gameObject);
        }

        private void Take(Explosion _explosion)
        {
            _explosion.gameObject.SetActive(true);
        }

        private void Return(Explosion _explosion)
        {
            _explosion.gameObject.SetActive(false);
        }
    }
}