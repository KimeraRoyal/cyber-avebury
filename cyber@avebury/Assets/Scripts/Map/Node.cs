using Niantic.Lightship.Maps.Core.Coordinates;
using UnityEngine;

namespace CyberAvebury
{
    public class Node : MonoBehaviour
    {
        private LatLng m_coordinates;

        public LatLng Coordinates => m_coordinates;

        public void AssignInformation(NodeInformation _information)
        {
            gameObject.name = _information.Name;

            m_coordinates = _information.Coordinates;
        }
    }
}
