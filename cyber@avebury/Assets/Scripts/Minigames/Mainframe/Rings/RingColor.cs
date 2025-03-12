using CyberAvebury.Minigames.Mainframe.Rings;
using UnityEngine;

namespace CyberAvebury
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class RingColor : MonoBehaviour
    {
        private Ring m_ring;

        private SpriteRenderer m_spriteRenderer;

        [SerializeField] private Gradient m_gradient;

        private void Awake()
        {
            m_ring = GetComponentInParent<Ring>();

            m_spriteRenderer = GetComponent<SpriteRenderer>();
            
            m_ring.OnLifetimeUpdated.AddListener(OnRingLifetimeUpdated);
        }

        private void OnRingLifetimeUpdated(float _lifetime)
        {
            var t = m_ring.Progress;
            
            m_spriteRenderer.color = m_gradient.Evaluate(t);
        }
    }
}
