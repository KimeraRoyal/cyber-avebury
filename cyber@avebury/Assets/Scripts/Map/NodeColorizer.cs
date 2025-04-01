using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

namespace CyberAvebury
{
    [RequireComponent(typeof(Node))]
    public class NodeColorizer : MonoBehaviour
    {
        private Node m_node;

        [SerializeField] private Color m_lockedColor;
        [SerializeField] private Color m_unlockedColor;
        [SerializeField] private Color m_completedColor;

        [SerializeField] private float m_colorChangeDuration = 0.5f;
        
        public UnityEvent<Color> OnColorUpdated;

        private Color m_currentColor;
        private Tween m_colorChangeTween;

        public Color CurrentColor
        {
            get => m_currentColor;
            private set
            {
                m_currentColor = value;
                OnColorUpdated?.Invoke(m_currentColor);
            }
        }
        
        private void Awake()
        {
            m_node = GetComponent<Node>();
            m_node.OnStateChanged.AddListener(OnNodeStateChanged);

            CurrentColor = m_lockedColor;
        }

        private void Start()
        {
            CurrentColor = m_lockedColor;
        }

        private void OnNodeStateChanged(NodeState _state)
        {
            if (m_colorChangeTween is { active: true })
            {
                m_colorChangeTween.Kill();
            }

            var color = _state switch
            {
                NodeState.Locked => m_lockedColor,
                NodeState.Unlocked => m_unlockedColor,
                NodeState.Completed => m_completedColor,
                _ => throw new ArgumentOutOfRangeException(nameof(_state), _state, null)
            };

            m_colorChangeTween = DOTween.To(() => CurrentColor, _color => CurrentColor = _color, color, m_colorChangeDuration);
        }
    }
}