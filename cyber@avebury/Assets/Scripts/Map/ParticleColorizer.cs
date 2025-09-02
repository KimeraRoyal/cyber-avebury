using System;
using UnityEngine;

namespace CyberAvebury
{
    [RequireComponent(typeof(ParticleSystem))]
    public class ParticleColorizer : MonoBehaviour
    {
        private ParticleSystem m_system;

        private bool m_justEnabled;
        
        private void Awake()
        {
            m_system = GetComponent<ParticleSystem>();
        }

        private void OnEnable()
        {
            m_justEnabled = true;
        }

        private void Update()
        {
            if(!m_justEnabled) { return; }
            m_justEnabled = false;
            
            var nodeColorizer = GetComponentInParent<NodeColorizer>();
            
            var mainModule = m_system.main;
            var startColor = mainModule.startColor;
            startColor.color = nodeColorizer.UnlockedColor;
            mainModule.startColor = startColor;
        }
    }
}
