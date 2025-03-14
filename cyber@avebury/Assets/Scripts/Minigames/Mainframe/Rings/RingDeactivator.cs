using CyberAvebury.Minigames.Mainframe.Rings;
using DG.Tweening;
using UnityEngine;

namespace CyberAvebury
{
    [RequireComponent(typeof(Ring))]
    public class RingDeactivator : MonoBehaviour
    {
        private RingParticles m_particles;
        
        private Ring m_ring;

        [SerializeField] private float m_shrinkScale = 0.9f;
        [SerializeField] private float m_shrinkDuration = 0.1f;

        private void Awake()
        {
            m_particles = FindAnyObjectByType<RingParticles>();
            
            m_ring = GetComponent<Ring>();
            
            m_ring.OnPressed.AddListener(OnRingPressed);
            m_ring.OnFailed.AddListener(OnRingFailed);
        }

        private void OnRingPressed(float _progress)
        {
            var targetScale = transform.localScale * m_shrinkScale;
            
            var sequence = DOTween.Sequence();
            sequence.Append(transform.DOScale(targetScale, m_shrinkDuration));
            sequence.AppendCallback(SpawnPressedParticles);
        }

        private void SpawnPressedParticles()
        {
            m_particles.SpawnPressedParticles(transform);
            m_ring.Deactivate();
        }

        private void OnRingFailed()
        {
            m_particles.SpawnFailedParticles(transform);
            m_ring.Deactivate();
        }
    }
}
