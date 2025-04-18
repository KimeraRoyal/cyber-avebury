using CyberAvebury.Minigames;
using DG.Tweening;
using UnityEngine;

namespace CyberAvebury
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class TwistRingBackground : MonoBehaviour
    {
        private Minigame m_minigame;

        private SpriteRenderer m_sprite;

        [SerializeField] private Color m_passedColor = Color.green;
        [SerializeField] private Color m_failedColor = Color.red;

        [SerializeField] private float m_colorChangeDuration = 0.5f;
        private Tween m_colorChangeTween;

        private void Awake()
        {
            m_minigame = GetComponentInParent<Minigame>();
            m_minigame.OnPassed.AddListener(OnPassed);
            m_minigame.OnFailed.AddListener(OnFailed);

            m_sprite = GetComponent<SpriteRenderer>();
        }

        private void OnPassed()
            => ChangeColor(m_passedColor);

        private void OnFailed()
            => ChangeColor(m_failedColor);

        private void ChangeColor(Color _color)
        {
            if(m_colorChangeTween is { active: true }) { m_colorChangeTween.Kill(); }
            m_colorChangeTween = m_sprite.DOColor(_color, m_colorChangeDuration);
        }
    }
}
