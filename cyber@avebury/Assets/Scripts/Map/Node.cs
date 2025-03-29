using System.Collections.Generic;
using Niantic.Lightship.Maps.Core.Coordinates;
using UnityEngine;

namespace CyberAvebury
{
    public class Node : MonoBehaviour
    {
        [SerializeField] private Transform m_lineAnchor;

        // TODO: Make connections two sided
        private HashSet<Node> m_connections;

        private NodeInfo m_info;

        public Transform LineAnchor => m_lineAnchor;

        public LatLng Coordinates => m_info.Coordinates;

        public HashSet<Node> Connections => m_connections;

        public NodeInfo Info => m_info;

        private void Awake()
        {
            m_connections = new HashSet<Node>();
        }

        public void AssignInformation(NodeInfo _info)
        {
            m_info = _info;
            
            gameObject.name = _info.name;
        }

        public void Connect(Node _node)
        {
            m_connections.Add(_node);
        }
    }
}
