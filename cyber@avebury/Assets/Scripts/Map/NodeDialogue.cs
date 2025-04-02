using System;
using UnityEngine;

namespace CyberAvebury
{
    [RequireComponent(typeof(Node))]
    public class NodeDialogue : MonoBehaviour
    {
        private Dialogue m_dialogue;
        
        private Node m_node;

        private void Awake()
        {
            m_dialogue = FindAnyObjectByType<Dialogue>();
            
            m_node = GetComponent<Node>();
            m_node.OnSelected.AddListener(OnNodeSelected);
            m_node.OnStateChanged.AddListener(OnStateChanged);
        }

        private void OnNodeSelected()
        {
            m_dialogue.AddLine(m_node.Info.SelectedLines);
        }

        private void OnStateChanged(NodeState _state)
        {
            switch (_state)
            {
                case NodeState.Locked:
                    break;
                case NodeState.Unlocked:
                    m_dialogue.AddLine(m_node.Info.UnlockedLines);
                    break;
                case NodeState.Completed:
                    m_dialogue.AddLine(m_node.Info.CompletedLines);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(_state), _state, null);
            }
        }
    }
}