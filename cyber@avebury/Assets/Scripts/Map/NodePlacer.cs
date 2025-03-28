using Niantic.Lightship.Maps.MapLayers.Components;
using UnityEngine;

namespace CyberAvebury
{
    [RequireComponent(typeof(Nodes))]
    public class NodePlacer : MonoBehaviour
    {
        private Nodes m_nodes;
        
        [SerializeField] private LayerGameObjectPlacement m_spawner;

        [SerializeField] private NodeInformation[] m_nodeInfo;

        private void Awake()
        {
            m_nodes = GetComponent<Nodes>();
        }

        private void Start()
        {
            foreach (var nodeInfo in m_nodeInfo)
            {
                var placedObject = m_spawner.PlaceInstance(nodeInfo.Coordinates, Quaternion.identity);
                
                var placedNode = placedObject.Value.GetComponent<Node>();
                placedNode.AssignInformation(nodeInfo);
                m_nodes.RegisterNode(placedNode);
            }
        }
    }
}
