using CyberAvebury.Minigames;
using UnityEngine;
using UnityEngine.Events;

namespace CyberAvebury
{
    public class NodeSelection : MonoBehaviour
    {
        private MinigameLoader m_loader;
        private Player m_player;

        [SerializeField] private float m_maxDistanceMeters = 30.0f;
        private float m_maxDistance = -1.0f;
        
        private Node m_selectedNode;

        private bool m_passedDirty;

        public float MaxDistance
        {
            get
            {
                if (m_maxDistance < 0.0f)
                {
                    m_maxDistance = m_player.MetersToScene(m_maxDistanceMeters);
                }
                return m_maxDistance;
            }
        }
        public float MaxDistanceMeters => m_maxDistanceMeters;

        public UnityEvent<Node> OnNodeSelected;
        public UnityEvent<Minigame> OnNodeMinigameLoaded;

        private void Awake()
        {
            m_loader = FindAnyObjectByType<MinigameLoader>();
            m_loader.OnMinigameLoaded.AddListener(OnMinigameLoaded);
            m_loader.OnMinigameUnloaded.AddListener(OnMinigameUnloaded);

            m_player = FindAnyObjectByType<Player>();
        }

        public void SelectNode(Node _node)
        {
            if(_node && m_player.GetDistanceToPoint(_node.transform.position) > m_maxDistanceMeters) { return; }

            m_selectedNode = _node;
            
            _node?.Select();
            OnNodeSelected?.Invoke(_node);
        }

        public void LoadSelectedMinigame()
        {
            if(!m_selectedNode) { return; }
            m_loader.LoadMinigame(m_selectedNode.Minigame);
        }

        private void OnMinigameLoaded(Minigame _minigame)
        {
            if (!_minigame) { return; }
            OnNodeMinigameLoaded?.Invoke(_minigame);

            _minigame.Begin(m_selectedNode.MinigameDifficulty);

            _minigame.OnPassed.AddListener(OnMinigamePassed);
            _minigame.OnEnd.AddListener(OnMinigameFinished);
        }

        private void OnMinigameUnloaded(Minigame _minigame)
        {
            _minigame.OnPassed.RemoveListener(OnMinigamePassed);
            _minigame.OnEnd.RemoveListener(OnMinigameFinished);
        }

        private void OnMinigamePassed()
        {
            m_passedDirty = true;
        }
    
        private void OnMinigameFinished()
        {
            if(!m_passedDirty) { return; }
            m_passedDirty = false;

            m_selectedNode.Complete();
            SelectNode(null);
        }
    }
}
