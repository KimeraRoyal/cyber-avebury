using UnityEngine;

namespace CyberAvebury
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class BossColorizer : MonoBehaviour
    {
        private Boss m_boss;

        private SpriteRenderer m_spriteRenderer;

        [SerializeField] private Gradient m_gradient;

        private void Awake()
        {
            m_boss = GetComponentInParent<Boss>();

            m_spriteRenderer = GetComponent<SpriteRenderer>();
            
            m_boss.OnMove.AddListener(OnBossMove);
        }

        private void OnBossMove(float _progress)
        {
            m_spriteRenderer.color = m_gradient.Evaluate(_progress);
        }
    }
}
