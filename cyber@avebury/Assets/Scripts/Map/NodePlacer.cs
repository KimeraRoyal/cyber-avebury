using UnityEngine;

namespace CyberAvebury
{
    [RequireComponent(typeof(Nodes))]
    public class NodePlacer : MonoBehaviour
    {
        private GPS m_gps;
        
        private Nodes m_nodes;

        [SerializeField] private Node m_nodePrefab;
        
        [SerializeField] private NodeInfo[] m_nodeInfo;

        private void Awake()
        {
            m_gps = FindAnyObjectByType<GPS>();
            
            m_nodes = GetComponent<Nodes>();
        }

        private void Start()
        {
            var nodes = new Node[m_nodeInfo.Length];
            for(var i = 0; i < m_nodeInfo.Length; i++)
            {
                var nodeInfo = m_nodeInfo[i];

                var placedNode = m_gps.PlaceObjectAt(m_nodePrefab, nodeInfo.Coordinates, Quaternion.identity);
                placedNode.transform.parent = transform;
                placedNode.AssignInformation(nodeInfo);
                nodes[i] = placedNode;
            }
            m_nodes.RegisterNodes(nodes);
        }
    }
}
