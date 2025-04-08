using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

namespace CyberAvebury
{
    public class Enemy : MonoBehaviour
    {
        private ProjectileTarget m_target;

        [SerializeField] private Vector3 m_startingPosition;
        [SerializeField] private float m_movementDuration = 1.0f;
        [SerializeField] private Ease m_movementEase = Ease.Linear;

        public UnityEvent OnDespawn;

        public void MoveToPosition(Vector3 _position)
        {
            var sequence = DOTween.Sequence();
            sequence.Append(transform.DOMove(_position, m_movementDuration).SetEase(m_movementEase));
            sequence.AppendCallback(() => m_target.Hittable = true);
        }

        private void Awake()
        {
            m_target = GetComponentInChildren<ProjectileTarget>();
            m_target.OnHit.AddListener(OnHit);
        }

        private void OnEnable()
        {
            transform.position = m_startingPosition;
            m_target.Hittable = false;
        }

        private void OnHit(Projectile _projectile)
        {
            OnDespawn?.Invoke();
        }
    }
}
