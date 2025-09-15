using UnityEngine;

namespace CyberAvebury
{
    public class Slingshot : MonoBehaviour
    {
        private InputHandling m_input;
        
        [SerializeField] private Camera m_camera;

        private ProjectilePool m_pool;
        private float m_distanceToCamera;

        [SerializeField] private AnimationCurve m_distanceCurve = AnimationCurve.Linear(0.0f, 0.0f, 1.0f, 1.0f);
        [SerializeField] private Vector3 m_distanceFactor = Vector3.one;
        [SerializeField] private float m_maxDistance = 1.0f;
        
        [SerializeField] private Vector2 m_aimCenter;
        [SerializeField] private Vector2 m_currentPosition;
        private bool m_aiming;

        private Projectile m_projectile;

        public Vector3 DistanceFactor
        {
            get => m_distanceFactor;
            set => m_distanceFactor = value;
        }

        private void Awake()
        {
            m_input = FindAnyObjectByType<InputHandling>();
            m_input.OnPress.AddListener(OnMousePressed);
            m_input.OnUnpress.AddListener(OnMouseReleased);
            
            m_pool = GetComponentInParent<ProjectilePool>();
        }

        private void Start()
        {
            m_distanceToCamera = (transform.position - m_camera.transform.position).magnitude;
        }

        private void OnMousePressed(Vector2 _position)
        {
            m_aimCenter = GetMouseWorldPosition();
            m_aiming = true;
        }

        private void OnMouseReleased(Vector2 _position)
        {
            Fire();
            m_aiming = false;
        }

        private void Update()
        {
            if (!m_projectile) { SpawnProjectile(); }
            
            if(!m_aiming) { return; }
            Aim(GetMouseWorldPosition());
        }

        private void Aim(Vector2 _targetPosition)
        {
            var difference = _targetPosition - m_aimCenter;
            var distance = m_distanceCurve.Evaluate(Mathf.Min(difference.magnitude, m_maxDistance) / m_maxDistance) * m_maxDistance;
            var normalizedDifference = difference.normalized;
            
            var targetOffset = new Vector3(
                normalizedDifference.x * distance * m_distanceFactor.x,
                normalizedDifference.y * distance * m_distanceFactor.y,
                distance * m_distanceFactor.z);
            m_projectile.transform.position = transform.position + targetOffset;
        }

        private void Fire()
        {
            if(!m_aiming) { return; }

            var difference = m_projectile.transform.position - transform.position;
            m_projectile.Fire(-difference.normalized);
            m_projectile = null;
        }

        private void SpawnProjectile()
        {
            m_projectile = m_pool.Get();
            m_projectile.transform.position = transform.position;
        }

        private Vector2 GetMouseWorldPosition()
            => m_camera.ScreenToWorldPoint(new Vector3(m_input.PointerPosition.x, m_input.PointerPosition.y, m_distanceToCamera));
    }
}
