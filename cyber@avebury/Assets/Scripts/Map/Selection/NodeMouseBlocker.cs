using UnityEngine;

namespace CyberAvebury
{
    public class NodeMouseBlocker : MonoBehaviour
    {
        private NodeSelection m_selection;
        private Mouse m_mouse;

        private bool m_locked;
        private bool m_nextLocked;
        
        private void Awake()
        {
            m_selection = FindAnyObjectByType<NodeSelection>();
            m_mouse = FindAnyObjectByType<Mouse>();
            
            m_selection.OnNodeSelected.AddListener(OnNodeSelected);
        }

        private void LateUpdate()
        {
            if(m_nextLocked == m_locked) { return; }
            m_locked = m_nextLocked;
            if (m_locked)
            {
                m_mouse.Lock();
            }
            else
            {
                m_mouse.Unlock();
            }
        }

        private void OnNodeSelected(Node _node)
        {
            m_nextLocked = _node;
        }
    }
}
