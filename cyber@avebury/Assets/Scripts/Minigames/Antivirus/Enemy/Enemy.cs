using System;
using CyberAvebury.Minigames;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

namespace CyberAvebury
{
    public class Enemy : MonoBehaviour
    {
        private Minigame m_minigame;
        private Antivirus m_antivirus;
        
        private ProjectileTarget m_target;

        [SerializeField] private Vector3 m_startingPosition;
        [SerializeField] private float m_movementDuration = 1.0f;
        [SerializeField] private Ease m_movementEase = Ease.Linear;

        [SerializeField] private float m_failAscendAmount = 10.0f;
        [SerializeField] private float m_failAscendDurationMin = 1.0f;
        [SerializeField] private float m_failAscendDurationMax = 1.0f;
        [SerializeField] private Ease m_failAscendEase = Ease.Linear;

        [SerializeField] private DifficultyAdjustedFloat m_lifetimeDifficulty = new(5.0f, 2.0f);
        private float m_lifetime;

        private float m_timer;

        private Sequence m_flyingSequence;
        private bool m_flyingOff;
        
        public UnityEvent OnDespawn;

        public void MoveToPosition(Vector3 _position)
        {
            if(m_flyingSequence is {active: true}) { m_flyingSequence?.Kill(); }
            
            m_flyingSequence = DOTween.Sequence();
            m_flyingSequence.Append(transform.DOMove(_position, m_movementDuration).SetEase(m_movementEase));
            m_flyingSequence.AppendCallback(() => m_target.Hittable = true);
        }

        private void Awake()
        {
            m_minigame = GetComponentInParent<Minigame>();
            m_antivirus = m_minigame.GetComponent<Antivirus>();
            
            m_minigame.OnDifficultySet.AddListener(OnDifficultySet);
            m_minigame.OnFailed.AddListener(FlyOff);
            
            m_target = GetComponentInChildren<ProjectileTarget>();
            m_target.OnHit.AddListener(OnHit);
        }

        private void Start()
        {
            OnDifficultySet(m_minigame.Difficulty);
        }

        private void OnEnable()
        {
            transform.position = m_startingPosition;
            m_target.Hittable = false;
            
            m_timer = 0.0f;

            if(m_flyingSequence is {active: true}) { m_flyingSequence?.Kill(); }
            m_flyingOff = false;
        }

        private void Update()
        {
            if(m_flyingOff) { return; }
            
            m_timer += Time.deltaTime;
            if(m_timer < m_lifetime) { return; }
            m_antivirus.ChangeScore(-1);
            FlyOff();
        }

        private void OnHit(Projectile _projectile)
        {
            m_antivirus.ChangeScore(1);
            OnDespawn?.Invoke();
        }

        private void OnDifficultySet(float _difficulty)
        {
            m_lifetime = m_lifetimeDifficulty.GetValue(_difficulty);
        }

        private void FlyOff()
        {
            if(m_flyingOff) { return; }
            m_flyingOff = true;
            
            if(m_flyingSequence is {active: true}) { m_flyingSequence?.Kill(); }
            
            m_flyingSequence = DOTween.Sequence();

            var endPosition = transform.position + Vector3.up * m_failAscendAmount;
            var duration = Random.Range(m_failAscendDurationMin, m_failAscendDurationMax);
            
            m_flyingSequence.Append(transform.DOMove(endPosition, duration).SetEase(m_failAscendEase));
            m_flyingSequence.AppendCallback(() => OnDespawn?.Invoke());
        }
    }
}
