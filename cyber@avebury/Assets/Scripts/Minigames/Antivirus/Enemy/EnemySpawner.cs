using System.Collections.Generic;
using CyberAvebury.Minigames;
using UnityEngine;
using Random = UnityEngine.Random;

namespace CyberAvebury
{
    [RequireComponent(typeof(EnemyPool))]
    public class EnemySpawner : MonoBehaviour
    {
        private Minigame m_minigame;
        
        private EnemyPool m_pool;
        private List<Enemy> m_enemies;

        [SerializeField] private Vector3 m_minStartPosition;
        [SerializeField] private Vector3 m_maxStartPosition;
        [SerializeField] private Vector3 m_minTargetPosition;
        [SerializeField] private Vector3 m_maxTargetPosition;

        [SerializeField] private DifficultyAdjustedFloat m_spawnTimeDifficulty = new(2.0f, 0.75f);
        private float m_spawnTime;
        private bool m_pause;

        private float m_timer;

        public IReadOnlyList<Enemy> Enemies => m_enemies;

        public bool Pause
        {
            get => m_pause;
            set => m_pause = value;
        }

        private void Awake()
        {
            m_minigame = GetComponentInParent<Minigame>();
            m_minigame.OnDifficultySet.AddListener(SetDifficulty);
            
            m_pool = GetComponent<EnemyPool>();
            m_enemies = new List<Enemy>();
        }

        private void Update()
        {
            if(!m_minigame.IsPlaying || m_pause) { return; }
            
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
            
            m_enemies.Add(enemy);
            enemy.OnDespawn.AddListener(OnEnemyDespawned);
        }

        private void OnEnemyDespawned(Enemy _enemy)
        {
            m_enemies.Remove(_enemy);
            _enemy.OnDespawn.RemoveListener(OnEnemyDespawned);
        }

        private void SetDifficulty(float _difficulty)
        {
            m_spawnTime = m_spawnTimeDifficulty.GetValue(_difficulty);
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