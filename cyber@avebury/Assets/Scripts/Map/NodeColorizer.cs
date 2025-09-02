using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

namespace CyberAvebury
{
    public class NodeColorizer : MonoBehaviour
    {
        private Node m_node;
        
        [SerializeField] private Color m_lockedColor;
        [SerializeField] private Color m_unlockedColor;
        [SerializeField] private Color m_unlockedSubnodeColor;
        [SerializeField] private Color m_completedColor;

        [SerializeField] private float m_colorChangeDuration = 0.5f;
        
        public UnityEvent<Color> OnColorUpdated;

        private bool m_isSubnode;

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

        public Color LockedColor => m_lockedColor;
        public Color UnlockedColor => m_unlockedColor;
        public Color UnlockedSubnodeColor => m_unlockedSubnodeColor;
        public Color CompletedColor => m_completedColor;
        
        private void Awake()
        {
            m_node = GetComponent<Node>();
            if (m_node)
            {
                m_node.OnStateChanged.AddListener(OnNodeStateChanged);
            }
            
            var miniObelisk = GetComponent<MiniObelisk>();
            if (miniObelisk)
            {
                miniObelisk.OnObeliskStateChanged.AddListener(OnNodeStateChanged);
            }

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
                NodeState.Unlocked => m_node && m_node.IsSubNode ? m_unlockedSubnodeColor : m_unlockedColor,
                NodeState.Completed => m_node && m_node.IsSubNode ? m_unlockedSubnodeColor : m_completedColor,
                _ => throw new ArgumentOutOfRangeException(nameof(_state), _state, null)
            };

            m_colorChangeTween = DOTween.To(() => CurrentColor, _color => CurrentColor = _color, color, m_colorChangeDuration);
        }
    }
}