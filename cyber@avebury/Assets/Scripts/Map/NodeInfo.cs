using Niantic.Lightship.Maps.Core.Coordinates;
using UnityEngine;

namespace CyberAvebury
{
    [CreateAssetMenu(fileName = "Node", menuName = "cyber@avebury/Node")]
    public class NodeInfo : ScriptableObject
    {
        [SerializeField] private string m_coordinates;
        private LatLng? m_storedCoordinates;

        [SerializeField] private NodeInfo[] m_connections;
        
        public LatLng Coordinates
        {
            get
            {
                if (m_storedCoordinates == null) { ConvertCoordinates(m_coordinates); }
                return m_storedCoordinates ?? new LatLng();
            }
        }

        public NodeInfo[] Connections => m_connections;

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