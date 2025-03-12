using CyberAvebury.Minigames.Mainframe;
using CyberAvebury.Minigames.Mainframe.Rings;
using UnityEngine;

namespace CyberAvebury
{
    [RequireComponent(typeof(RingSpawner))]
    public class RingScorer : MonoBehaviour
    {
        private Mainframe m_mainframe;
        
        private RingSpawner m_spawner;

        [SerializeField] private int m_earliestRingScore = 5;
        [SerializeField] private int m_latestRingScore = 1;
        [SerializeField] private AnimationCurve m_ringScoringCurve = AnimationCurve.Linear(0, 0, 1, 1);
        
        private void Awake()
        {
            m_mainframe = GetComponentInParent<Mainframe>();

            m_spawner = GetComponent<RingSpawner>();
            
            m_spawner.OnRingSpawned.AddListener(OnRingSpawned);
        }

        private void OnRingSpawned(Ring _ring)
        {
            _ring.OnPressed.AddListener(OnRingPressed);
        }

        private void OnRingPressed(float _progress)
            => m_mainframe.AddScore(GetRingScore(_progress));

        private int GetRingScore(float _progress)
        {
            var curvedProgress = m_ringScoringCurve.Evaluate(_progress);
            return Mathf.CeilToInt((m_latestRingScore - m_earliestRingScore) * curvedProgress) + m_earliestRingScore;
        }
    }
}
