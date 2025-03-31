using System;
using UnityEngine;

namespace CyberAvebury
{
    public class NodeMouseBlocker : MonoBehaviour
    {
        private NodeSelection m_selection;
        private Mouse m_mouse;

        private bool m_locked;
        private bool m_lockedDirty;
        
        private void Awake()
        {
            m_selection = FindAnyObjectByType<NodeSelection>();
            m_mouse = FindAnyObjectByType<Mouse>();
            
            m_selection.OnNodeSelected.AddListener(OnNodeSelected);
            m_mouse.OnMouseClicked.AddListener(OnMouseClicked);
        }

        private void LateUpdate()
        {
            if(!m_lockedDirty) { return; }

            m_locked = !m_locked;
            if (m_locked)
            {
                m_mouse.Lock();
            }
            else
            {
                m_mouse.Unlock();
            }
            
            m_lockedDirty = false;
        }

        private void OnNodeSelected(Node _node)
        {
            if (_node && !m_locked)
            {
                m_lockedDirty = true;
            }
            else if(!_node && m_locked)
            {
                m_lockedDirty = true;
            }
        }

        private void OnMouseClicked(Vector2Int _position)
        {
            if(!m_locked) { return; }
            m_selection.SelectNode(null);
        }
    }
}
