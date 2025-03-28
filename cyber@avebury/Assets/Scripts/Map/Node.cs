using Niantic.Lightship.Maps.Core.Coordinates;
using UnityEngine;

namespace CyberAvebury
{
    public class Node : MonoBehaviour
    {
        [SerializeField] private Transform m_lineAnchor;
        
        private LatLng m_coordinates;

        public Transform LineAnchor => m_lineAnchor;

        public LatLng Coordinates => m_coordinates;

        public void AssignInformation(NodeInformation _information)
        {
            gameObject.name = _information.Name;

            m_coordinates = _information.Coordinates;
        }
    }
}
