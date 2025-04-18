using CyberAvebury.Minigames;
using DG.Tweening;
using UnityEngine;

namespace CyberAvebury
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class TwistIndicator : MonoBehaviour
    {
        private Minigame m_minigame;
        private TwistRings m_rings;

        private SpriteRenderer m_sprite;

        [SerializeField] private float m_verticalIncrement = -0.75f;
        [SerializeField] private float m_verticalMovementDuration = 0.5f;
        [SerializeField] private Ease m_verticalMovementEase = Ease.Linear;

        [SerializeField] private Color m_validColor = Color.green;
        [SerializeField] private Color m_invalidColor = Color.black;

        [SerializeField] private float m_fadeOutDuration = 0.1f;

        private float m_initialY;
        private Tween m_verticalPositionTween;

        private void Awake()
        {
            m_minigame = GetComponentInParent<Minigame>();
            m_minigame.OnFinished.AddListener(OnMinigameFinished);

            m_rings = m_minigame.GetComponentInChildren<TwistRings>();
            m_rings.OnCurrentRingChanged.AddListener(OnCurrentRingChanged);

            m_sprite = GetComponent<SpriteRenderer>();

            m_initialY = transform.localPosition.y;
        }

        private void Start()
        {
            var position = transform.localPosition;
            position.y = m_initialY + m_verticalIncrement * m_rings.CurrentRingIndex;
            transform.localPosition = position;
        }

        private void Update()
        {
            if(!m_minigame.IsPlaying) { return; }
            var valid = m_rings.CurrentRing && m_rings.CurrentRing.IsActive && m_rings.CurrentRing.IsAngleValid;
            var color = valid ? m_validColor : m_invalidColor;
            m_sprite.color = color;
        }

        private void OnCurrentRingChanged(int _currentRingIndex)
        {
            if(m_verticalPositionTween is { active: true}) { m_verticalPositionTween.Kill(); }

            var targetY = m_initialY + m_verticalIncrement * _currentRingIndex;
            m_verticalPositionTween = transform.DOLocalMoveY(targetY, m_verticalMovementDuration).SetEase(m_verticalMovementEase);
        }

        private void OnMinigameFinished()
        {
            m_sprite.DOFade(0.0f, m_fadeOutDuration);
        }
    }
}
