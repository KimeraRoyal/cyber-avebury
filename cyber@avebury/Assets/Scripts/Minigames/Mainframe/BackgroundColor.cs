using System;
using CyberAvebury.Minigames.Mainframe;
using UnityEngine;

namespace CyberAvebury
{
    [RequireComponent(typeof(Camera))]
    public class BackgroundColor : MonoBehaviour
    {
        private Mainframe m_mainframe;
        
        private Camera m_camera;

        [SerializeField] private Gradient m_gradient;

        private void Awake()
        {
            m_mainframe = GetComponentInParent<Mainframe>();
            
            m_camera = GetComponent<Camera>();
            
            m_mainframe.OnScoreUpdated.AddListener(OnScoreUpdated);
        }

        private void Start()
            => UpdateColor(0);

        private void OnScoreUpdated(int _score)
            => UpdateColor(m_mainframe.ScoreProgress);

        private void UpdateColor(float _t)
            => m_camera.backgroundColor = m_gradient.Evaluate(_t);
    }
}
