using System;
using UnityEngine;
using UnityEngine.UI;

namespace CyberAvebury
{
    [RequireComponent(typeof(CanvasScaler))]
    public class ScaleToCamera : MonoBehaviour
    {
        private enum Axis
        {
            Width,
            Height
        }
        
        private CanvasScaler m_scaler;

        [SerializeField] private Axis m_comparisonAxis;

        [SerializeField] private float m_scaleFactor = 1.0f;
        [SerializeField] private float m_referenceLength;

        private int m_previousScreenLength;
        
        private void Awake()
        {
            m_scaler = GetComponent<CanvasScaler>();
        }

        private void Update()
        {
            var screenLength = GetScreenLength();
            if(screenLength == m_previousScreenLength) { return; }
            m_previousScreenLength = screenLength;

            var factor = Mathf.Max(1, Mathf.Floor(screenLength / m_referenceLength));
            m_scaler.scaleFactor = factor * m_scaleFactor;
        }

        private int GetScreenLength()
            => m_comparisonAxis switch
            {
                Axis.Width => Screen.width,
                Axis.Height => Screen.height,
                _ => throw new ArgumentOutOfRangeException()
            };
    }
}
