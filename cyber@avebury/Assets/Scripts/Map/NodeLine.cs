using UnityEngine;

namespace CyberAvebury
{
    [RequireComponent(typeof(LineRenderer))]
    public class NodeLine : MonoBehaviour
    {
        private LineRenderer m_lineRenderer;

        [SerializeField] [Range(2, 10)] private int m_segments = 2;
        
        private Vector3[] m_positions;
        
        private Node m_a;
        private Node m_b;

        public void Connect(Node _a, Node _b)
        {
            m_a = _a;
            m_b = _b;
        }

        private void Awake()
        {
            m_lineRenderer = GetComponent<LineRenderer>();
        }

        private void Update()
        {
            if(!(m_a && m_b)) { return; }
            
            UpdateSegmentCount();
            UpdateLinePositions();
        }

        private void UpdateLinePositions()
        {
            var aPos = m_a.LineAnchor.position;
            var bPos = m_b.LineAnchor.position;
            
            var distance = bPos - aPos;
            var step = distance / (m_segments - 1);
            
            for (var i = 0; i < m_segments; i++)
            {
                m_positions[i] = aPos + step * i;
            }
            m_lineRenderer.SetPositions(m_positions);
        }

        private void UpdateSegmentCount()
        {
            if(m_lineRenderer.positionCount == m_segments) { return; }
            
            m_lineRenderer.positionCount = m_segments;
            m_positions = new Vector3[m_segments];
        }
    }
}
