using System;
using UnityEngine;
using UnityEngine.UI;

namespace CyberAvebury
{
    [RequireComponent(typeof(CanvasScaler))]
    public class ScaleCanvasToScreen : MonoBehaviour
    {
        private enum Axis
        {
            Width,
            Height
        }
        
        private enum RoundingBehaviour
        {
            None,
            Floor,
            Round,
            Ceiling
        }
        
        private CanvasScaler m_scaler;

        [SerializeField] private int m_scaleFactor = 1;
        [SerializeField] private Axis m_axis = Axis.Height;
        [SerializeField] private int m_targetSize = 480;
        
        [SerializeField] private RoundingBehaviour m_roundingBehaviour;

        private int m_lastScreenSize;
        private int m_lastTargetSize;
        private RoundingBehaviour m_lastRoundingBehaviour;

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
            if(GetScreenSize() == m_lastScreenSize || m_targetSize == m_lastTargetSize && m_roundingBehaviour == m_lastRoundingBehaviour) { return; }
            
            Scale();
            m_lastScreenSize = GetScreenSize();
            m_lastTargetSize = m_targetSize;
            m_lastRoundingBehaviour = m_roundingBehaviour;
        }

        private void Scale()
        {
            var screenFactor = GetScreenSize() / (float)m_targetSize;
            var screenFactorRounded = m_roundingBehaviour switch
            {
                RoundingBehaviour.None => (int)screenFactor,
                RoundingBehaviour.Floor => Mathf.RoundToInt(screenFactor),
                RoundingBehaviour.Round => Mathf.RoundToInt(screenFactor),
                RoundingBehaviour.Ceiling => Mathf.CeilToInt(screenFactor),
                _ => throw new ArgumentOutOfRangeException()
            };
            m_scaler.scaleFactor = Math.Max(1, m_scaleFactor * screenFactorRounded);
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
