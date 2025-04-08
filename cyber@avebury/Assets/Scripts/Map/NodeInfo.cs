using CyberAvebury.Minigames;
using Niantic.Lightship.Maps.Core.Coordinates;
using NUnit.Framework;
using UnityEngine;

namespace CyberAvebury
{
    [CreateAssetMenu(fileName = "Node", menuName = "cyber@avebury/Node")]
    public class NodeInfo : ScriptableObject
    {
        [SerializeField] private string m_coordinates;
        private LatLng? m_storedCoordinates;

        [SerializeField] private NodeInfo[] m_connections;

        [SerializeField] private NodeState m_defaultState = NodeState.Locked;

        [SerializeField] private Minigame m_minigame;
        [SerializeField] [UnityEngine.Range(0.0f, 1.0f)] private float m_minigameDifficulty = 0.0f;

        [SerializeField] private DialogueLinesObject m_unlockedLines;
        [SerializeField] private DialogueLinesObject m_selectedLines;
        [SerializeField] private DialogueLinesObject m_completedLines;
        
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

        private void ConvertCoordinates(string _coordinates)
        {
            var splitCoordinates = _coordinates.Split(", ");
            if(splitCoordinates.Length != 2) { return; }
            
            if(!double.TryParse(splitCoordinates[0], out var latitude)) { return; }
            if(!double.TryParse(splitCoordinates[1], out var longitude)) { return; }
            m_storedCoordinates = new LatLng(latitude, longitude);
        }
    }
}