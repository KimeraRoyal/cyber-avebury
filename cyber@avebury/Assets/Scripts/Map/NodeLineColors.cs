using System;
using UnityEngine;

namespace CyberAvebury
{
    [Serializable]
    public class NodeLineColors
    {
        [SerializeField] private Color m_lockedMainColor;
        [SerializeField] private Color m_lockedSecondaryColor;

        [SerializeField] private Color m_unlockedMainColor;
        [SerializeField] private Color m_unlockedSecondaryColor;

        [SerializeField] private Color m_completedMainColor;
        [SerializeField] private Color m_completedSecondaryColor;

        public Color LockedMainColor => m_lockedMainColor;
        public Color LockedSecondaryColor => m_lockedSecondaryColor;

        public Color UnlockedMainColor => m_unlockedMainColor;
        public Color UnlockedSecondaryColor => m_unlockedSecondaryColor;

        public Color CompletedMainColor => m_completedMainColor;
        public Color CompletedSecondaryColor => m_completedSecondaryColor;

        public void GetColors(NodeState _state, out Color _a, out Color _b)
        {
            switch (_state)
            {
                case NodeState.Locked:
                    _a = m_lockedMainColor;
                    _b = m_lockedSecondaryColor;
                    break;
                case NodeState.Unlocked:
                    _a = m_unlockedMainColor;
                    _b = m_unlockedSecondaryColor;
                    break;
                case NodeState.Completed:
                    _a = m_completedMainColor;
                    _b = m_completedSecondaryColor;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}