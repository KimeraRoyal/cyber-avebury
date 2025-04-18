using CyberAvebury.Minigames;
using DG.Tweening;
using UnityEngine;

namespace CyberAvebury
{
    public class TwistCameraZoom : MonoBehaviour
    {
        private Minigame m_minigame;
        private TwistRings m_rings;

        [SerializeField] private float m_increment = -0.75f;
        [SerializeField] private float m_movementDuration = 0.5f;
        [SerializeField] private Ease m_movementEase = Ease.Linear;

        [SerializeField] private float m_finishMovementDuration = 1.0f;
        [SerializeField] private Ease m_finishMovementEase = Ease.Linear;

        private float m_initialZ;
        private Tween m_positionTween;

        private void Awake()
        {
            m_minigame = GetComponentInParent<Minigame>();
            m_minigame.OnFinished.AddListener(OnMinigameFinished);

            m_rings = m_minigame.GetComponentInChildren<TwistRings>();
            m_rings.OnCurrentRingChanged.AddListener(OnCurrentRingChanged);

            m_initialZ = transform.localPosition.z;
        }

        private void Start()
        {
            var position = transform.localPosition;
            position.z = m_initialZ + m_increment * m_rings.CurrentRingIndex;
            transform.localPosition = position;
        }

        private void OnCurrentRingChanged(int _currentRingIndex)
        {
            var targetZ = m_initialZ + m_increment * _currentRingIndex;
            MoveCamera(targetZ, m_movementDuration, m_movementEase);
        }

        private void OnMinigameFinished()
        {
            MoveCamera(m_initialZ, m_finishMovementDuration, m_finishMovementEase);
        }

        private void MoveCamera(float _z, float _duration, Ease _ease)
        {
            if (m_positionTween is { active: true }) { m_positionTween.Kill(); }

            m_positionTween = transform.DOLocalMoveZ(_z, _duration).SetEase(_ease);
        }
    }
}
