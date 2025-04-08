using UnityEngine;
using Random = UnityEngine.Random;

namespace CyberAvebury
{
    [RequireComponent(typeof(EnemyPool))]
    public class EnemySpawner : MonoBehaviour
    {
        private EnemyPool m_pool;

        [SerializeField] private Vector3 m_minStartPosition;
        [SerializeField] private Vector3 m_maxStartPosition;
        [SerializeField] private Vector3 m_minTargetPosition;
        [SerializeField] private Vector3 m_maxTargetPosition;
        [SerializeField] private float m_spawnTime = 1.0f;

        private float m_timer;

        private void Awake()
        {
            m_pool = GetComponent<EnemyPool>();
        }

        private void Update()
        {
            m_timer += Time.deltaTime;
            if (m_timer < m_spawnTime) { return; }
            m_timer -= m_spawnTime;
            
            SpawnEnemy();
        }

        private void SpawnEnemy()
        {
            var startPosition = GetRandomVector(m_minStartPosition, m_maxStartPosition);
            var targetPosition = GetRandomVector(m_minTargetPosition, m_maxTargetPosition);

            var enemy = m_pool.Get();
            enemy.transform.position = startPosition;
            enemy.MoveToPosition(targetPosition);
        }

        private static Vector3 GetRandomVector(Vector3 _min, Vector3 _max)
            => new()
            {
                x = Random.Range(_min.x, _max.x),
                y = Random.Range(_min.y, _max.y),
                z = Random.Range(_min.z, _max.z)
            };
    }
}