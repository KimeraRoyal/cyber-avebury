using System;
using UnityEngine;

namespace CyberAvebury
{
    public class NodeBoundary : MonoBehaviour
    {
        private enum ApproachState
        {
            Far,
            Approaching,
            Near
        }

        private NodeSelection m_selection;
        private Player m_player;

        private SpriteRenderer[] m_renderers;

        [SerializeField] private Gradient m_approachGradient;
        [SerializeField] private float m_visibleRange = 1.5f;

        [SerializeField] private Vector3 m_scale = Vector3.one;

        private ApproachState m_currentState;

        private void Awake()
        {
            m_selection = FindAnyObjectByType<NodeSelection>();
            m_player = FindAnyObjectByType<Player>();

            m_renderers = GetComponentsInChildren<SpriteRenderer>();
        }

        private void Start()
        {
            transform.localScale = m_scale * m_selection.MaxDistance;
            
            CalculateDistance(true);
        }

        private void Update()
        {
            CalculateDistance(false);
        }

        private void CalculateDistance(bool _forceStateChange)
        {
            var distance = Vector2.Distance(new Vector2(transform.position.x, transform.position.z), m_player.GetFlatPosition());

            var state = ApproachState.Far;
            if (distance <= m_selection.MaxDistance) { state = ApproachState.Near; }
            else if (distance <= m_selection.MaxDistance * m_visibleRange) { state = ApproachState.Approaching; }

            if (!_forceStateChange && state == m_currentState && state != ApproachState.Approaching) { return; }

            UpdateState(state, distance);
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
