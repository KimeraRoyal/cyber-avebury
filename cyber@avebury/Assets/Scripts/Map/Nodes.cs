using System;
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

        public UnityEvent<Node> OnNodeUnlocked;
        public UnityEvent<Node> OnNodeCompleted;

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
            _node.OnStateChanged.AddListener(_state => OnNodeStateChanged(_node, _state));

            OnNodeRegistered?.Invoke(_node);
        }

        private void OnNodeStateChanged(Node _node, NodeState _state)
        {
            switch (_state)
            {
                case NodeState.Locked:
                    break;
                case NodeState.Unlocked:
                    OnNodeUnlocked?.Invoke(_node);
                    break;
                case NodeState.Completed:
                    OnNodeCompleted?.Invoke(_node);
                    break;
            }
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
