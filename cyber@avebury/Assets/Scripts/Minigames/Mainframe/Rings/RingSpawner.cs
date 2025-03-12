using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

namespace CyberAvebury.Minigames.Mainframe.Rings
{
    [RequireComponent(typeof(RingPool))]
    public class RingSpawner : MonoBehaviour
    { 
        private Minigame m_minigame;
        private RingArea m_ringArea;

        private RingPool m_ringPool;

        [SerializeField] private float m_initialSpawnTime = 0.1f;
        [SerializeField] private DifficultyAdjustedFloat m_minSpawnTimeDifficulty = new(1.0f, 0.25f);
        [SerializeField] private DifficultyAdjustedFloat m_maxSpawnTimeDifficulty = new(2.0f, 0.75f);

        private float m_minSpawnTime;
        private float m_maxSpawnTime;
        private float m_spawnTime;
        private float m_timer;

        public UnityEvent<Ring> OnRingSpawned;

        private void Awake()
        {
            m_minigame = GetComponentInParent<Minigame>();
            m_ringArea = FindAnyObjectByType<RingArea>();

            m_ringPool = GetComponent<RingPool>();
    
            m_minigame.OnDifficultySet.AddListener(SetDifficulty);
        }

        private void Start()
        {
            m_spawnTime = m_initialSpawnTime;
        }

        private void OnDestroy()
        {
            m_minigame.OnDifficultySet.RemoveListener(SetDifficulty);
        }

        private void Update()
        {
            if(!m_minigame.IsPlaying) { return; }
            
            m_timer += Time.deltaTime;
            if(m_timer < m_spawnTime) { return; }
            m_timer -= m_spawnTime;

            m_spawnTime = Random.Range(m_minSpawnTime, m_maxSpawnTime);
            SpawnRing();
        }

        private void SpawnRing()
        {
            var ring = m_ringPool.Get();
            ring.transform.position = m_ringArea.GetRandomPosition();
            
            OnRingSpawned?.Invoke(ring);
        }

        private void SetDifficulty(float _difficulty)
        {
            m_minSpawnTime = m_minSpawnTimeDifficulty.GetValue(_difficulty);
            m_maxSpawnTime = m_maxSpawnTimeDifficulty.GetValue(_difficulty);
        }
    }
}
