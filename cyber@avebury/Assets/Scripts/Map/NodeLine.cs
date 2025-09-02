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

        [SerializeField] private NodeLineColors m_defaultColors;
        
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

            var aLineColors = m_a.Info.OverwriteLineColors ? m_a.Info.LineColors : m_defaultColors;
            aLineColors.GetColors(state, out var aMainColor, out var aSecondaryColor);
            
            var bLineColors = m_b.Info.OverwriteLineColors ? m_b.Info.LineColors : m_defaultColors;
            bLineColors.GetColors(state, out var bMainColor, out var bSecondaryColor);

            var gradient = m_lineRenderer.colorGradient;
            var colorKeys = gradient.colorKeys;
            colorKeys[0].color = aMainColor;
            colorKeys[1].color = Color.Lerp(aSecondaryColor, bSecondaryColor, 0.5f);
            colorKeys[2].color = bMainColor;
            gradient.SetKeys(colorKeys, gradient.alphaKeys);
            m_lineRenderer.colorGradient = gradient;
            
            m_colorsDirty = false;
        }
    }
}
