using System;
using System.Collections.Generic;
using CyberAvebury.Minigames;
using Niantic.Lightship.Maps.Core.Coordinates;
using UnityEngine;

namespace CyberAvebury
{
    public class Node : MonoBehaviour
    {
        [SerializeField] private Transform m_lineAnchor;

        private HashSet<Node> m_connections;

        private NodeInfo m_info;

        public Transform LineAnchor => m_lineAnchor;

        public LatLng Coordinates => m_info.Coordinates;

        public HashSet<Node> Connections => m_connections;

        public Minigame Minigame => m_info.Minigame;
        public float MinigameDifficulty => m_info.MinigameDifficulty;

        public NodeInfo Info => m_info;

        public static Action<Node, Node> OnNodesConnected;

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
            if(!(m_connections.Add(_node) && _node.m_connections.Add(this))) { return; }
            Debug.Log($"Connected {gameObject.name} to {_node.gameObject.name}");
            OnNodesConnected?.Invoke(this, _node);
        }
    }
}
