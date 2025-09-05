using System;
using UnityEngine;

namespace CyberAvebury
{
    [RequireComponent(typeof(Node))]
    public class NodeMusic : MonoBehaviour
    {
        private MusicPlayer m_music;
        
        private Node m_node;
        
        private void Awake()
        {
            m_music = FindAnyObjectByType<MusicPlayer>();
            
            m_node = GetComponent<Node>();
            m_node.OnStateChanged.AddListener(OnStateChanged);
            m_node.OnEntered.AddListener(OnEntered);
        }

        private void OnEntered()
        {
            m_music.PlaySong(m_node.Info.EnteredMusic);
        }

        private void OnStateChanged(NodeState _state)
        {
            switch (_state)
            {
                case NodeState.Locked:
                    break;
                case NodeState.Unlocked:
                    m_music.PlaySong(m_node.Info.Music);
                    break;
                case NodeState.Completed:
                    m_music.PlaySong(m_node.Info.CompletedMusic);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(_state), _state, null);
            }
        }
    }
}
