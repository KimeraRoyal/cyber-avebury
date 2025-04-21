using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Events;

namespace CyberAvebury
{
    public class Projectile : MonoBehaviour
    {
        private Camera m_camera;

        [SerializeField] private float m_lifetime = 5.0f;
        [SerializeField] private float m_speed = 1.0f;

        [SerializeField] private LayerMask m_mask;
        [SerializeField] private float m_radius = 0.1f;
        
        private Vector3 m_direction;
        private bool m_fired;

        private float m_timer;

        public UnityEvent<ProjectileTarget> OnHitTarget;
        public UnityEvent OnDespawn;
        
        public Camera Camera
        {
            get => m_camera;
            set => m_camera = value;
        }

        public void Fire(Vector3 _direction)
        {
            m_direction = _direction;
            m_fired = true;
        }

        private void OnEnable()
        {
            m_direction = Vector3.zero;
            m_fired = false;
            
            m_timer = 0.0f;
        }

        private void Update()
        {
            if(!m_fired) { return; }
            
            TickLifetime();
            
            transform.position += m_direction * (m_speed * Time.deltaTime);
            CheckCollision();
        }

        private void CheckCollision()
        {
            var ray = new Ray(m_camera.transform.position, (transform.position - m_camera.transform.position).normalized);
            
            Debug.DrawRay(ray.origin, ray.direction * m_camera.farClipPlane, Color.red);
            if(!Physics.SphereCast(ray, m_radius, out var hitInfo, m_camera.farClipPlane, m_mask)) { return; }

            var target = hitInfo.transform.GetComponent<ProjectileTarget>();
            if (!target || !target.Hit(this)) { return; }
            OnHitTarget?.Invoke(target);
            OnDespawn?.Invoke();
        }

        private void TickLifetime()
        {
            m_timer += Time.deltaTime;
            if(m_timer < m_lifetime) { return; }
            
            m_direction = Vector3.zero;
            m_fired = false;
            OnDespawn?.Invoke();
        }
    }
}
