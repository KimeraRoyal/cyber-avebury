using DG.Tweening;
using UnityEngine;

namespace CyberAvebury
{
    public class Enemy : MonoBehaviour
    {
        private ProjectileTarget m_target;

        [SerializeField] private float m_movementDuration = 1.0f;
        [SerializeField] private Ease m_movementEase = Ease.Linear;
        [SerializeField] private Vector3 m_targetPosition;

        private void Awake()
        {
            m_target = GetComponentInChildren<ProjectileTarget>();
            m_target.OnHit.AddListener(OnHit);
        }

        private void Start()
        {
            m_target.Hittable = false;
            var sequence = DOTween.Sequence();
            sequence.Append(transform.DOMove(m_targetPosition, m_movementDuration).SetEase(m_movementEase));
            sequence.AppendCallback(() => m_target.Hittable = true);
        }

        private void OnHit(Projectile _projectile)
        {
            Destroy(gameObject);
        }
    }
}
