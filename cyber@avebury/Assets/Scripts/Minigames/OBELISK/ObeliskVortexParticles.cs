using System;
using UnityEngine;

namespace CyberAvebury
{
    [RequireComponent(typeof(ParticleSystem))]
    public class ObeliskVortexParticles : MonoBehaviour
    {
        private Obelisk m_obelisk;
        
        private ParticleSystem m_particles;
        
        private void Awake()
        {
            m_obelisk = GetComponentInParent<Obelisk>();
            
            m_particles = GetComponent<ParticleSystem>();
            
            m_obelisk.OnBeginLoadingSubgame.AddListener(OnBeginLoadingSubgame);
        }

        private void OnDisable()
        {
            m_particles.Stop();
        }

        private void OnBeginLoadingSubgame(int _particleIndex)
        {
            m_particles.Play();
        }
    }
}
