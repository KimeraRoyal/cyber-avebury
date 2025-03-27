using Niantic.Lightship.Maps.Core.Coordinates;
using Niantic.Lightship.Maps.MapLayers.Components;
using UnityEngine;

namespace CyberAvebury
{
    public class NodePlacer : MonoBehaviour
    {
        [SerializeField] private LayerGameObjectPlacement m_spawner;

        [SerializeField] private double m_nodeLatitude;
        [SerializeField] private double m_nodeLongitude;
        
        private void Start()
        {
            m_spawner.PlaceInstance(new LatLng(m_nodeLatitude, m_nodeLongitude), Quaternion.identity);
        }
    }
}
