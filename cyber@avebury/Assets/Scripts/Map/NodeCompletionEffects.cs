using System.Collections;
using UnityEngine;

namespace CyberAvebury
{
    public class NodeCompletionEffects : MonoBehaviour
    {
        private Nodes m_nodes;

        [SerializeField] private ParticlePool m_unlockEffectPool;
        [SerializeField] private ParticlePool m_completionEffectPool;

        [SerializeField] private float m_unlockDelay = 1.2f;
        [SerializeField] private float m_completionDelay = 1.0f;

        private void Awake()
        {
            m_nodes = GetComponentInParent<Nodes>();
            m_nodes.OnNodeUnlocked.AddListener(OnNodeUnlocked);
            m_nodes.OnNodeCompleted.AddListener(OnNodeCompleted);
        }

        private void OnNodeUnlocked(Node _node)
        {
            StartCoroutine(SpawnUnlockEffect(_node));
        }

        private void OnNodeCompleted(Node _node)
        {
            StartCoroutine(SpawnCompletedEffect(_node));
        }

        private IEnumerator SpawnUnlockEffect(Node _node)
        {
            yield return new WaitForSeconds(m_unlockDelay);

            var particles = m_unlockEffectPool.Get();
            particles.transform.SetParent(_node.LineAnchor);
            particles.transform.localPosition = Vector3.zero;
            particles.transform.localRotation = Quaternion.identity;
            particles.transform.localScale = Vector3.one;
        }

        private IEnumerator SpawnCompletedEffect(Node _node)
        {
            yield return new WaitForSeconds(m_completionDelay);

            var particles = m_completionEffectPool.Get();
            particles.transform.position = _node.transform.position;
        }
    }
}
