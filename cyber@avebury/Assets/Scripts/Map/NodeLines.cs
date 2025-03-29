using UnityEngine;

namespace CyberAvebury
{
    [RequireComponent(typeof(Nodes))]
    public class NodeLines : MonoBehaviour
    {
        private Nodes m_nodes;

        [SerializeField] private NodeLine m_linePrefab;

        private void Awake()
        {
            m_nodes = GetComponent<Nodes>();
            m_nodes.OnFinishedRegistering.AddListener(SpawnLines);
        }

        private void SpawnLines()
        {
            //TODO: Prevent spawning two lines in instances where nodes share a mutual connection.
            foreach (var nodePair in m_nodes.RegisteredNodes)
            {
                var node = nodePair.Value;
                foreach (var connection in node.Connections)
                {
                    SpawnLine(node, connection);
                }
            }
        }

        private void SpawnLine(Node _a, Node _b)
        {
            var line = Instantiate(m_linePrefab, transform);
            line.Connect(_a, _b);
        }
    }
}
