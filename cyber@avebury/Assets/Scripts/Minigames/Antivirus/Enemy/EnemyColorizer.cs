using DG.Tweening;
using UnityEngine;

namespace CyberAvebury
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class EnemyColorizer : MonoBehaviour
    {
        private SpriteRenderer m_sprite;
        
        [SerializeField] private Color m_startingColor = Color.black;
        [SerializeField] private Color m_targetColor = Color.green;
        [SerializeField] private float m_colorTweenDuration = 1.0f;
        [SerializeField] private Ease m_colorTweenEase = Ease.Linear;

        private void Awake()
        {
            m_sprite = GetComponent<SpriteRenderer>();
        }

        private void OnEnable()
        {
            m_sprite.color = m_startingColor;
            m_sprite.DOColor(m_targetColor, m_colorTweenDuration).SetEase(m_colorTweenEase);
        }
    }
}
