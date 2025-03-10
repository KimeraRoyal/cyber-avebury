using UnityEngine;
using Random = UnityEngine.Random;

namespace CyberAvebury.Minigame.Mainframe.Rings
{
    public class RingSpawner : MonoBehaviour
    { 
        private MinigameBase m_minigameBase;
        private RingArea m_ringArea;

        [SerializeField] private Ring m_ringPrefab;

        [SerializeField] private float m_initialSpawnTime = 0.1f;
        [SerializeField] private DifficultyAdjustedFloat m_minSpawnTimeDifficulty = new(1.0f, 0.25f);
        [SerializeField] private DifficultyAdjustedFloat m_maxSpawnTimeDifficulty = new(2.0f, 0.75f);
            
        [SerializeField] private DifficultyAdjustedFloat m_ringMaxSizeDifficulty = new (2.0f, 1.0f);
        [SerializeField] private DifficultyAdjustedFloat m_ringLifetimeDifficulty = new (5.0f, 1.5f);

        private float m_minSpawnTime;
        private float m_maxSpawnTime;
        private float m_spawnTime;
        private float m_timer;

        private float m_ringMaxSize;
        private float m_ringLifetime;

        private void Awake()
        {
            m_minigameBase = GetComponentInParent<MinigameBase>();
            m_ringArea = FindAnyObjectByType<RingArea>();

            m_minigameBase.OnDifficultySet += SetDifficulty;
        }

        private void Start()
        {
            m_spawnTime = m_initialSpawnTime;
        }

        private void OnDestroy()
        {
            m_minigameBase.OnDifficultySet -= SetDifficulty;
        }

        private void Update()
        {
            m_timer += Time.deltaTime;
            if(m_timer < m_spawnTime) { return; }
            m_timer -= m_spawnTime;

            m_spawnTime = Random.Range(m_minSpawnTime, m_maxSpawnTime);
            SpawnRing();
        }

        private void SpawnRing()
        {
            var position = m_ringArea.GetRandomPosition();
            var ring = Instantiate(m_ringPrefab, position, Quaternion.identity, transform);
            
            ring.MaxSize = m_ringMaxSize;
            ring.TotalLifetime = m_ringLifetime;
        }

        private void SetDifficulty(float _difficulty)
        {
            m_minSpawnTime = m_minSpawnTimeDifficulty.GetValue(_difficulty);
            m_maxSpawnTime = m_maxSpawnTimeDifficulty.GetValue(_difficulty);
            
            m_ringMaxSize = m_ringMaxSizeDifficulty.GetValue(_difficulty);
            m_ringLifetime = m_ringLifetimeDifficulty.GetValue(_difficulty);
        }
    }
}
