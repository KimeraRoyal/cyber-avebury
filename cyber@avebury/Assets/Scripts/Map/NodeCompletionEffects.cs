using UnityEngine;

namespace CyberAvebury
{
    public class NodeCompletionEffects : MonoBehaviour
    {
        private Nodes m_nodes;

        [SerializeField] private ParticlePool m_unlockEffectPool;
        [SerializeField] private ParticlePool m_completionEffectPool;

        private void Awake()
        {
            m_nodes = GetComponentInParent<Nodes>();
            m_nodes.OnNodeUnlocked.AddListener(OnNodeUnlocked);
            m_nodes.OnNodeCompleted.AddListener(OnNodeCompleted);
        }

        private void OnNodeUnlocked(Node _node)
        {
            var particles = m_unlockEffectPool.Get();
            particles.transform.SetParent(_node.LineAnchor);
            particles.transform.localPosition = Vector3.zero;
            particles.transform.localRotation = Quaternion.identity;
            particles.transform.localScale = Vector3.one;
        }

        private void OnNodeCompleted(Node _node)
        {
            var particles = m_completionEffectPool.Get();
            particles.transform.position = _node.transform.position;
        }
    }
}
