using UnityEngine;

namespace CyberAvebury
{
    public class NodeLines : MonoBehaviour
    {
        [SerializeField] private NodeLine m_linePrefab;

        private void Awake()
        {
            Node.OnNodesConnected += SpawnLine;
        }

        private void SpawnLine(Node _a, Node _b)
        {
            var line = Instantiate(m_linePrefab, transform);
            line.Connect(_a, _b);
        }
    }
}
