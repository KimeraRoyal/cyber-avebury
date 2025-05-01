using System;
using UnityEngine;
using UnityEngine.UI;

namespace CyberAvebury
{
    [RequireComponent(typeof(CanvasScaler))]
    public class UnclampedScaleCanvasToScreen : MonoBehaviour
    {
        private enum Axis
        {
            Width,
            Height
        }
        
        private CanvasScaler m_scaler;

        [SerializeField] private float m_scaleFactor = 1;
        [SerializeField] private Axis m_axis = Axis.Height;
        [SerializeField] private int m_targetSize = 480;
        
        private int m_lastScreenSize;
        private int m_lastTargetSize;

        private void Awake()
        {
            m_scaler = GetComponent<CanvasScaler>();
        }

        private void Start()
        {
            Scale();
        }

        private void Update()
        {
            if(GetScreenSize() == m_lastScreenSize && m_targetSize == m_lastTargetSize) { return; }
            
            Scale();
            m_lastScreenSize = GetScreenSize();
            m_lastTargetSize = m_targetSize;
        }

        private void Scale()
        {
            var screenFactor = GetScreenSize() / (float)m_targetSize;
            m_scaler.scaleFactor = Math.Max(1, screenFactor * m_scaleFactor);
        }

        private int GetScreenSize() 
            => m_axis switch
            {
                Axis.Width => Screen.width,
                Axis.Height => Screen.height,
                _ => throw new ArgumentOutOfRangeException()
            };
    }
}
