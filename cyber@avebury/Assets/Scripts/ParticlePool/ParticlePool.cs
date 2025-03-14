using UnityEngine;
using UnityEngine.Pool;

namespace CyberAvebury
{
    public class ParticlePool : MonoBehaviour
    {
        private IObjectPool<ParticleSystem> m_pool;

        [SerializeField] private ParticleSystem m_prefab;
        
        private void Awake()
        {
            m_pool = new ObjectPool<ParticleSystem>(Create, Take, Return, Destroy, true, 10, 100);
        }

        public ParticleSystem Get()
            => m_pool.Get();

        public void Release(ParticleSystem _particles)
            => m_pool.Release(_particles);

        private ParticleSystem Create()
        {
            var particles = Instantiate(m_prefab, transform);
            particles.gameObject.AddComponent<ReleaseParticlesToPool>();
            return particles;
        }

        private void Destroy(ParticleSystem _particles)
        {
            Destroy(_particles.gameObject);
        }

        private void Take(ParticleSystem _particles)
        {
            _particles.gameObject.SetActive(true);
            _particles.Play();
        }

        private void Return(ParticleSystem _particles)
        {
            _particles.Stop();
            _particles.gameObject.SetActive(false);
        }
    }
}
