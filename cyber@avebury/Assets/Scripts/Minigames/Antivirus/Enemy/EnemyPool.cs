using UnityEngine;
using UnityEngine.Pool;

namespace CyberAvebury
{
    public class EnemyPool : MonoBehaviour
    {
        private IObjectPool<Enemy> m_pool;

        [SerializeField] private Enemy m_enemyPrefab;

        private void Awake()
        {
            m_pool = new ObjectPool<Enemy>(Create, Take, Return, Destroy, true, 10, 100);
        }

        public Enemy Get()
            => m_pool.Get();

        public void Release(Enemy _enemy)
            => m_pool.Release(_enemy);

        private Enemy Create()
        {
            var enemy = Instantiate(m_enemyPrefab, transform);
            enemy.OnDespawn.AddListener(Release);
            return enemy;
        }

        private void Destroy(Enemy _enemy)
        {
            Destroy(_enemy.gameObject);
        }

        private void Take(Enemy _enemy)
        {
            _enemy.gameObject.SetActive(true);
        }

        private void Return(Enemy _enemy)
        {
            _enemy.gameObject.SetActive(false);
        }
    }
}
