using System;
using UnityEngine;

namespace CyberAvebury
{
    [RequireComponent(typeof(Node))]
    public class NodeMusic : MonoBehaviour
    {
        private Node m_node;
        
        private void Awake()
        {
            m_node = GetComponent<Node>();
            m_node.OnStateChanged.AddListener(OnStateChanged);
            m_node.OnBeginComplete.AddListener(OnBeginComplete);
            m_node.OnEntered.AddListener(OnEntered);
        }

        private void OnBeginComplete()
        {
            if(m_node.Info.PlayCompletedMusicImmediately) { MusicPlayer.Instance.ChangeMusicState(m_node.Info.CompletedMusic); }
            if(!m_node.Info.StopCurrentSongOnCompletion) { return; }
            MusicPlayer.Instance.StopSong();
        }

        private void OnEntered()
        {
            MusicPlayer.Instance.ChangeMusicState(m_node.Info.EnteredMusic);
        }

        private void OnStateChanged(NodeState _state)
        {
            switch (_state)
            {
                case NodeState.Locked:
                    break;
                case NodeState.Unlocked:
                    MusicPlayer.Instance.ChangeMusicState(m_node.Info.Music);
                    break;
                case NodeState.Completed:
                    if(!m_node.Info.PlayCompletedMusicImmediately) { MusicPlayer.Instance.ChangeMusicState(m_node.Info.CompletedMusic); }
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(_state), _state, null);
            }
        }
    }
}
