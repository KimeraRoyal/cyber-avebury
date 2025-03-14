using UnityEngine;

namespace CyberAvebury.Minigames.Mainframe.Rings
{
    public class RingParticles : MonoBehaviour
    {
        [SerializeField] private ParticlePool m_pressedParticles;
        [SerializeField] private ParticlePool m_failedParticles;

        public void SpawnPressedParticles(Transform _ringTransform)
        {
            SpawnParticles(m_pressedParticles, _ringTransform);
        }

        public void SpawnFailedParticles(Transform _ringTransform)
        {
            SpawnParticles(m_failedParticles, _ringTransform);
        }

        private void SpawnParticles(ParticlePool _pool, Transform _ringTransform)
        {
            var particles = _pool.Get();

            particles.transform.position = _ringTransform.position;

            var shape = particles.shape;
            shape.scale = _ringTransform.localScale;
        }
    }
}