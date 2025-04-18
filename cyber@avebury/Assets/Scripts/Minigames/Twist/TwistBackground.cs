using CyberAvebury.Minigames;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace CyberAvebury
{
    [RequireComponent(typeof(Tilemap))]
    public class TwistBackground : MonoBehaviour
    {
        private Minigame m_minigame;

        private Tilemap m_tilemap;

        [SerializeField] private Color m_passedColor = Color.green;

        [SerializeField] private float m_colorChangeDuration = 0.5f;
        private Tween m_colorChangeTween;

        private void Awake()
        {
            m_minigame = GetComponentInParent<Minigame>();
            m_minigame.OnPassed.AddListener(OnPassed);

            m_tilemap = GetComponent<Tilemap>();
        }

        private void OnPassed()
            => ChangeColor(m_passedColor);

        private void ChangeColor(Color _color)
        {
            if (m_colorChangeTween is { active: true }) { m_colorChangeTween.Kill(); }
            m_colorChangeTween = DOTween.To(() => m_tilemap.color, _value => m_tilemap.color = _value, _color, m_colorChangeDuration);
        }
    }
}
