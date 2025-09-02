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

        [SerializeField] private float m_colorTransitionDuration = 1.0f;

        [SerializeField] private NodeLineColors m_defaultColors;
        
        private Vector3[] m_positions;
        private bool m_colorsDirty;
        
        private Node m_a;
        private Node m_b;

        private Tween m_colorTransition;
        private float m_colorTransitionProgress;
        private GradientColorKey[] m_colorsFrom;
        private GradientColorKey[] m_colorsTo;

        private float ColorTransitionProgress
        {
            get => m_colorTransitionProgress;
            set
            {
                m_colorTransitionProgress = value;
                UpdateColorTransition();
            }
        }

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
            UpdateColors(false);
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

        private void OnNodeStateChanged(NodeState _node)
        {
            m_colorsDirty = true;
            UpdateColors(false);
        }

        private void UpdateColors(bool _instant)
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
            m_colorsFrom = gradient.colorKeys;
            m_colorsTo = gradient.colorKeys;
            m_colorsTo[0].color = aMainColor;
            m_colorsTo[1].color = Color.Lerp(aSecondaryColor, bSecondaryColor, 0.5f);
            m_colorsTo[2].color = bMainColor;
            
            if (_instant)
            {
                gradient.SetKeys(m_colorsTo, gradient.alphaKeys);
                m_lineRenderer.colorGradient = gradient;
            }
            else
            {
                if(m_colorTransition is { active: true }) { m_colorTransition.Kill(); }

                m_colorTransitionProgress = 0.0f;
                m_colorTransition = DOTween.To(() => ColorTransitionProgress, _value => ColorTransitionProgress = _value, 1.0f, m_colorTransitionDuration);
            }
            
            m_colorsDirty = false;
        }

        private void UpdateColorTransition()
        {
            var gradient = m_lineRenderer.colorGradient;
            var colorKeys = gradient.colorKeys;
            for (var i = 0; i < colorKeys.Length; i++)
            {
                colorKeys[i].color = Color.Lerp(m_colorsFrom[i].color, m_colorsTo[i].color, m_colorTransitionProgress);
            }
            gradient.SetKeys(colorKeys, gradient.alphaKeys);
            m_lineRenderer.colorGradient = gradient;
        }
    }
}
