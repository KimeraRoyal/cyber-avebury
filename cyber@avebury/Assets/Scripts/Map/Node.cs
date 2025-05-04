using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CyberAvebury.Minigames;
using Niantic.Lightship.Maps.Core.Coordinates;
using UnityEngine;
using UnityEngine.Events;

namespace CyberAvebury
{
    public class Node : MonoBehaviour
    {
        [SerializeField] private Transform m_lineAnchor;
        [SerializeField] private float m_completionUnlockDelay = 1.0f;

        private NodeState m_currentState;

        private HashSet<Node> m_connections;

        private NodeInfo m_info;

        public Transform LineAnchor => m_lineAnchor;

        public LatLng Coordinates => m_info.Coordinates;

        public NodeState CurrentState
        {
            get => m_currentState;
            set
            {
                if(m_currentState == value) { return; }
                m_currentState = value;
                OnStateChanged?.Invoke(m_currentState);
            }
        }

        public HashSet<Node> Connections => m_connections;

        public Minigame Minigame => m_info.Minigame;
        public float MinigameDifficulty => m_info.MinigameDifficulty;
        public bool IsSubNode => !Minigame;

        public NodeInfo Info => m_info;

        public static Action<Node, Node> OnNodesConnected;

        public UnityEvent OnSelected;

        public UnityEvent<NodeState> OnStateChanged;

        private void Awake()
        {
            m_connections = new HashSet<Node>();
        }

        public void AssignInformation(NodeInfo _info)
        {
            m_info = _info;
            
            gameObject.name = _info.name;
            CurrentState = _info.DefaultState;
        }

        public void Connect(Node _node)
        {
            if(!(m_connections.Add(_node) && _node.m_connections.Add(this))) { return; }
            OnNodesConnected?.Invoke(this, _node);
        }

        public void Select()
        {
            OnSelected?.Invoke();
        }

        // TODO: It'd be cool to point the camera at nodes and unlock them one by one. But like. lol
        public void Unlock()
        {
            if (CurrentState != NodeState.Locked) { return; }
            CurrentState = NodeState.Unlocked;
            
            if(!IsSubNode) { return; }
            Complete();
        }

        public void Complete()
        {
            if(CurrentState == NodeState.Completed) { return; }
            CurrentState = NodeState.Completed;
            StartCoroutine(UnlockNeighbours());
        }

        private IEnumerator UnlockNeighbours()
        {
            yield return new WaitForSeconds(m_completionUnlockDelay);
            foreach (var connection in m_connections.Where(_connection => _connection.CurrentState == NodeState.Locked))
            {
                connection.Unlock();
            }
        }

        public Vector2 GetFlatPosition()
            => new (transform.position.x, transform.position.z);
    }
}
