using DG.Tweening;
using System;
using UnityEngine;

namespace CyberAvebury
{
    [RequireComponent(typeof(LineRenderer))]
    public class NodeLine : MonoBehaviour
    {
        private LineRenderer m_lineRenderer;

        [SerializeField] [Range(2, 10)] private int m_segments = 2;

        [SerializeField] private Color m_lockedMainColor;
        [SerializeField] private Color m_lockedSecondaryColor;

        [SerializeField] private Color m_unlockedMainColor;
        [SerializeField] private Color m_unlockedSecondaryColor;

        [SerializeField] private Color m_completedMainColor;
        [SerializeField] private Color m_completedSecondaryColor;
        
        private Vector3[] m_positions;
        private bool m_colorsDirty;
        
        private Node m_a;
        private Node m_b;

        public void Connect(Node _a, Node _b)
        {
            if (m_a || m_b)
            {
                Debug.LogError("Tried to connect line nodes twice.");
                return;
            }
            
            m_a = _a;
            m_b = _b;
            
            m_a.OnStateChanged.AddListener(OnNodeStateChanged);
            m_b.OnStateChanged.AddListener(OnNodeStateChanged);
            m_colorsDirty = true;
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
            UpdateColors();
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

        private void OnNodeStateChanged(NodeState _node)
        {
            m_colorsDirty = true;
            UpdateColors();
        }

        // TODO: Animate this gradient change
        private void UpdateColors()
        {
            if(!m_colorsDirty) { return; }
            
            var state = NodeState.Completed;
            if (m_a.CurrentState == NodeState.Locked || m_b.CurrentState == NodeState.Locked)
            {
                state = NodeState.Locked;
            }
            else if (m_a.CurrentState == NodeState.Unlocked || m_b.CurrentState == NodeState.Unlocked)
            {
                state = NodeState.Unlocked;
            }

            Color mainColor;
            Color secondaryColor;
            switch (state)
            {
                case NodeState.Locked:
                    mainColor = m_lockedMainColor;
                    secondaryColor = m_lockedSecondaryColor;
                    break;
                case NodeState.Unlocked:
                    mainColor = m_unlockedMainColor;
                    secondaryColor = m_unlockedSecondaryColor;
                    break;
                case NodeState.Completed:
                    mainColor = m_completedMainColor;
                    secondaryColor = m_completedSecondaryColor;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            var gradient = m_lineRenderer.colorGradient;
            var colorKeys = gradient.colorKeys;
            colorKeys[0].color = mainColor;
            colorKeys[1].color = secondaryColor;
            colorKeys[2].color = mainColor;
            gradient.SetKeys(colorKeys, gradient.alphaKeys);
            m_lineRenderer.colorGradient = gradient;
            
            m_colorsDirty = false;
        }
    }
}
