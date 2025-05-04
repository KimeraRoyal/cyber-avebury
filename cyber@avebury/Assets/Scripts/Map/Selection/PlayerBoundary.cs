using System;
using UnityEngine;

namespace CyberAvebury
{
    public class PlayerBoundary : MonoBehaviour
    {
        private enum ApproachState
        {
            Far,
            Approaching,
            Near
        }

        private NodeSelection m_selection;
        private Nodes m_nodes;
        
        private Player m_player;

        private SpriteRenderer[] m_renderers;

        [SerializeField] private Gradient m_approachGradient;
        [SerializeField] private float m_visibleRange = 1.5f;

        [SerializeField] private Vector3 m_scale = Vector3.one;

        private ApproachState m_currentState;

        private void Awake()
        {
            m_selection = FindAnyObjectByType<NodeSelection>();
            m_nodes = FindAnyObjectByType<Nodes>();
            
            m_player = GetComponentInParent<Player>();

            m_renderers = GetComponentsInChildren<SpriteRenderer>();
            
            m_nodes.OnFinishedRegistering.AddListener(OnFinishedRegisteringNodes);
        }

        private void Start()
        {
            transform.localScale = m_scale * m_selection.MaxDistance;
        }

        private void OnFinishedRegisteringNodes()
        {
            CalculateDistance(true);
        }
        
        private void Update()
        {
            CalculateDistance(false);
        }

        private void CalculateDistance(bool _forceStateChange)
        {
            Node closestNode = null;
            var closestDistance = float.MaxValue;
            foreach (var node in m_nodes.NodeList)
            {
                if(node.CurrentState == NodeState.Locked) { continue; }
                
                var distanceToNode = Vector2.Distance(node.GetFlatPosition(), m_player.GetFlatPosition());
                if(distanceToNode >= closestDistance) { continue; }

                closestNode = node;
                closestDistance = distanceToNode;
            }
            
            if(!closestNode) { return; }

            var state = ApproachState.Far;
            if (closestDistance <= m_selection.MaxDistance) { state = ApproachState.Near; }
            else if (closestDistance <= m_selection.MaxDistance * m_visibleRange) { state = ApproachState.Approaching; }

            if (!_forceStateChange && state == m_currentState && state != ApproachState.Approaching) { return; }

            UpdateState(state, closestDistance);
        }

        private void UpdateState(ApproachState _state, float _distance)
        {
            switch (_state)
            {
                case ApproachState.Far:
                    AssignColor(m_approachGradient.Evaluate(0.0f));
                    break;
                case ApproachState.Approaching:
                    var t = (_distance - m_selection.MaxDistance) / (m_selection.MaxDistance * m_visibleRange - m_selection.MaxDistance);
                    t = 1.0f - Mathf.Clamp(t, 0.0f, 1.0f);
                    AssignColor(m_approachGradient.Evaluate(t));
                    break;
                case ApproachState.Near:
                    AssignColor(m_approachGradient.Evaluate(1.0f));
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            m_currentState = _state;
        }

        private void AssignColor(Color _color)
        {
            foreach (var renderer in m_renderers)
            {
                renderer.color = _color;
            }
        }
    }
}
