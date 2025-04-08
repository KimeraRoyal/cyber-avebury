using UnityEngine;
using UnityEngine.Pool;

namespace CyberAvebury
{
    public class ProjectilePool : MonoBehaviour
    {
        [SerializeField] private Camera m_camera;

        private IObjectPool<Projectile> m_pool;

        [SerializeField] private Projectile m_projectilePrefab;

        private void Awake()
        {
            m_pool = new ObjectPool<Projectile>(Create, Take, Return, Destroy, true, 10, 100);
        }

        public Projectile Get()
            => m_pool.Get();

        public void Release(Projectile _projectile)
            => m_pool.Release(_projectile);

        private Projectile Create()
        {
            var projectile = Instantiate(m_projectilePrefab, transform);
            projectile.Camera = m_camera;
            projectile.OnDespawn.AddListener(() => Release(projectile));
            return projectile;
        }

        private void Destroy(Projectile _projectile)
        {
            Destroy(_projectile.gameObject);
        }

        private void Take(Projectile _projectile)
        {
            _projectile.gameObject.SetActive(true);
        }

        private void Return(Projectile _projectile)
        {
            _projectile.gameObject.SetActive(false);
        }
    }
}
