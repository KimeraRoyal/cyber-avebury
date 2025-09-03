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
        }

        private void OnStateChanged(NodeState _state)
        {
            if(_state != NodeState.Unlocked) { return; }
            m_music.PlaySong(m_node.Info.Music);
        }
    }
}
