using System.Collections;
using UnityEngine;

namespace CyberAvebury
{
    [RequireComponent(typeof(ParticleSystem))]
    public class ReleaseParticlesToPool : MonoBehaviour
    {
        private ParticlePool m_pool;
        
        private ParticleSystem m_particles;

        private void Awake()
        {
            m_pool = GetComponentInParent<ParticlePool>();
            
            m_particles = GetComponent<ParticleSystem>();
        }

        private void OnEnable()
        {
            StartCoroutine(WaitForParticles());
        }

        private IEnumerator WaitForParticles()
        {
            yield return new WaitForSeconds(m_particles.main.duration);
            m_pool.Release(m_particles);
        }
    }
}
