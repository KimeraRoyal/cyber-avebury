using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace CyberAvebury
{
    public class Nodes : MonoBehaviour
    {
        private Dictionary<string, Node> m_nodes;

        public IReadOnlyDictionary<string, Node> RegisteredNodes => m_nodes;

        public UnityEvent<Node> OnNodeRegistered;
        public UnityEvent OnFinishedRegistering;

        private void Awake()
        {
            m_nodes = new Dictionary<string, Node>();
        }

        public void RegisterNodes(Node[] _nodes)
        {
            foreach (var node in _nodes)
            {
                RegisterNode(node);
            }
            ConnectNodes();
                
            OnFinishedRegistering?.Invoke();
        }

        private void RegisterNode(Node _node)
        {
            m_nodes.Add(_node.name, _node);
            OnNodeRegistered?.Invoke(_node);
        }

        private void ConnectNodes()
        {
            foreach (var nodePair in m_nodes)
            {
                var node = nodePair.Value; 
                foreach (var connection in node.Info.Connections)
                {
                    node.Connect(m_nodes[connection.name]);
                }
            }
        }
    }
}
