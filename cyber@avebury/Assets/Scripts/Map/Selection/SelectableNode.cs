using System;
using TouchScript.Gestures;
using UnityEngine;

namespace CyberAvebury
{
    [RequireComponent(typeof(Node))]
    public class SelectableNode : MonoBehaviour
    {
        private Dialogue m_dialogue;
        
        private NodeSelection m_selection;
        
        private Node m_node;
        private TapGesture m_tap;

        private void Awake()
        {
            m_dialogue = FindAnyObjectByType<Dialogue>();
            m_selection = FindAnyObjectByType<NodeSelection>();
            
            m_node = GetComponent<Node>();
            m_tap = GetComponentInChildren<TapGesture>();
            
            m_tap.Tapped += Tapped;
        }

        private void Tapped(object _sender, EventArgs _e)
        {
            OnClicked();
        }

        private void OnClicked()
        {
            if(m_node.IsSubNode || m_node.CurrentState == NodeState.Locked || m_dialogue.HasDialogue) { return; }
            m_selection.SelectNode(m_node);
        }
    }
}
