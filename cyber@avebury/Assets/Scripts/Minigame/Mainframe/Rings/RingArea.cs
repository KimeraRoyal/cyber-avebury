using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace CyberAvebury.Minigame.Mainframe.Rings
{
    public class RingArea : MonoBehaviour
    {
        [SerializeField] private Camera m_camera;

        [SerializeField] private Vector2 m_edgeBufferSize;

        private Vector2 m_minBound;
        private Vector2 m_maxBound;

        public Vector2 EdgeBufferSize => m_edgeBufferSize;

        private void Awake()
        {
            m_minBound = CalculateMinBound();
            m_maxBound = CalculateMaxBound();
        }

        public Vector2 GetRandomPosition()
            => new(Random.Range(m_minBound.x, m_maxBound.x), Random.Range(m_minBound.y, m_maxBound.y));

        public Vector2 CalculateMinBound()
        {
            Vector2 minBound= m_camera.ViewportToWorldPoint(Vector2.zero);
                
            // TODO: I guess this breaks if you rotate the camera?
            minBound += m_edgeBufferSize;

            return minBound;
        }

        public Vector2 CalculateMaxBound()
        {
            Vector2 maxBound= m_camera.ViewportToWorldPoint(Vector2.one);
                
            // TODO: I guess this breaks if you rotate the camera?
            maxBound -= m_edgeBufferSize;

            return maxBound;
        }
    }
}
