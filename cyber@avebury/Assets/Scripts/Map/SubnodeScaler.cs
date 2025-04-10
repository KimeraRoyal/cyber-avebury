using UnityEngine;

namespace CyberAvebury
{
    public class SubnodeScaler : MonoBehaviour
    {
        private Node m_node;

        [SerializeField] private Vector3 m_scaleIfSubnode = Vector3.one;

        private void Awake()
        {
            m_node = GetComponentInParent<Node>();
        }

        private void Start()
        {
            if(!m_node.IsSubNode) { return; }

            transform.localScale = m_scaleIfSubnode;
        }
    }
}
