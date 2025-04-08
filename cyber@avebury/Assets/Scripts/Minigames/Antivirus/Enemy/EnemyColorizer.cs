using DG.Tweening;
using UnityEngine;

namespace CyberAvebury
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class EnemyColorizer : MonoBehaviour
    {
        private SpriteRenderer m_sprite;

        [SerializeField] private Gradient m_colorGradient;
        [SerializeField] private float m_colorTweenDuration = 1.0f;
        [SerializeField] private Ease m_colorTweenEase = Ease.Linear;

        private float m_gradientPosition;

        private float GradientPosition
        {
            get => m_gradientPosition;
            set
            {
                m_gradientPosition = value;
                m_sprite.color = m_colorGradient.Evaluate(m_gradientPosition);
            }
        }

        private void Awake()
        {
            m_sprite = GetComponent<SpriteRenderer>();
        }

        private void OnEnable()
        {
            GradientPosition = 0.0f;
            DOTween.To(() => GradientPosition, _value => GradientPosition = _value, 1.0f, m_colorTweenDuration).SetEase(m_colorTweenEase);
        }
    }
}
