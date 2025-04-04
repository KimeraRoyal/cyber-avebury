using UnityEngine;

namespace CyberAvebury
{
    public class LogoParticles : MonoBehaviour
    {
        [SerializeField] private ParticleSystem m_particles;

        private void Awake()
        {
            m_particles = GetComponentInChildren<ParticleSystem>();
        }

        public void PlayParticles()
        {
            m_particles?.Play();
        }
    }
}
