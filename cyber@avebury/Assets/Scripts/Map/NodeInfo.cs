using CyberAvebury.Minigames;
using FMODUnity;
using UnityEngine;

namespace CyberAvebury
{
    [CreateAssetMenu(fileName = "Node", menuName = "cyber@avebury/Node")]
    public class NodeInfo : ScriptableObject
    {
        [SerializeField] private string m_coordinates;
        private LatLng? m_storedCoordinates;

        [SerializeField] private Node m_prefab;
        
        [SerializeField] private NodeInfo[] m_connections;

        [SerializeField] private NodeState m_defaultState = NodeState.Locked;

        [SerializeField] private Minigame m_minigame;
        [SerializeField] [Range(0.0f, 1.0f)] private float m_minigameDifficulty = 0.0f;

        [SerializeField] private DialogueLinesObject m_unlockedLines;
        [SerializeField] private DialogueLinesObject m_selectedLines;
        [SerializeField] private DialogueLinesObject m_completedLines;

        [SerializeField] private bool m_overwriteLineColors;
        [SerializeField] private NodeLineColors m_lineColors;

        [SerializeField] private EventReference m_music;
        [SerializeField] private EventReference m_enteredMusic;
        [SerializeField] private EventReference m_completedMusic;

        public Node Prefab => m_prefab;
        
        public LatLng Coordinates
        {
            get
            {
                if (m_storedCoordinates == null) { ConvertCoordinates(m_coordinates); }
                return m_storedCoordinates ?? new LatLng();
            }
        }

        public NodeInfo[] Connections => m_connections;

        public NodeState DefaultState => m_defaultState;

        public Minigame Minigame => m_minigame;
        public float MinigameDifficulty => m_minigameDifficulty;

        public DialogueLinesObject UnlockedLines => m_unlockedLines;
        public DialogueLinesObject SelectedLines => m_selectedLines;
        public DialogueLinesObject CompletedLines => m_completedLines;

        public bool OverwriteLineColors => m_overwriteLineColors;
        public NodeLineColors LineColors => m_lineColors;

        public EventReference Music => m_music;
        public EventReference EnteredMusic => m_enteredMusic;
        public EventReference CompletedMusic => m_completedMusic;

        private void ConvertCoordinates(string _coordinates)
        {
            if(!LatLng.FromString(_coordinates, out var coordinates)) { return; }
            m_storedCoordinates = coordinates;
        }
    }
}