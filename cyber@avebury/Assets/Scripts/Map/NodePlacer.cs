using Niantic.Lightship.Maps.MapLayers.Components;
using UnityEngine;

namespace CyberAvebury
{
    [RequireComponent(typeof(Nodes))]
    public class NodePlacer : MonoBehaviour
    {
        private Nodes m_nodes;
        
        [SerializeField] private LayerGameObjectPlacement m_spawner;

        [SerializeField] private NodeInfo[] m_nodeInfo;

        private void Awake()
        {
            m_nodes = GetComponent<Nodes>();
        }

        private void Start()
        {
            var nodes = new Node[m_nodeInfo.Length];
            for(var i = 0; i < m_nodeInfo.Length; i++)
            {
                var nodeInfo = m_nodeInfo[i];
                
                var placedObject = m_spawner.PlaceInstance(nodeInfo.Coordinates, Quaternion.identity);
                
                var placedNode = placedObject.Value.GetComponent<Node>();
                placedNode.AssignInformation(nodeInfo);
                nodes[i] = placedNode;
            }
            m_nodes.RegisterNodes(nodes);
        }
    }
}
