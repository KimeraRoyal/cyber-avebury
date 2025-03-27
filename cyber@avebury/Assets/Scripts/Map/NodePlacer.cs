using Niantic.Lightship.Maps.MapLayers.Components;
using UnityEngine;

namespace CyberAvebury
{
    public class NodePlacer : MonoBehaviour
    {
        [SerializeField] private LayerGameObjectPlacement m_spawner;

        [SerializeField] private NodeInformation[] m_nodeInfo;
        
        private void Start()
        {
            foreach (var nodeInfo in m_nodeInfo)
            {
                var placedObject = m_spawner.PlaceInstance(nodeInfo.Coordinates, Quaternion.identity);
                
                var placedNode = placedObject.Value.GetComponent<Node>();
                placedNode.AssignInformation(nodeInfo);
            }
        }
    }
}
