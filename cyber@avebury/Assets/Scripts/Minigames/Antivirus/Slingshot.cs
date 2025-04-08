using UnityEngine;

namespace CyberAvebury
{
    public class Slingshot : MonoBehaviour
    {
        [SerializeField] private Camera m_camera;
        
        [SerializeField] private Transform m_anchor;
        [SerializeField] private Transform m_target;
        private float m_anchorDistance;

        [SerializeField] private AnimationCurve m_distanceCurve = AnimationCurve.Linear(0.0f, 0.0f, 1.0f, 1.0f);
        [SerializeField] private Vector3 m_distanceFactor = Vector3.one;
        [SerializeField] private float m_maxDistance = 1.0f;
        
        [SerializeField] private Vector2 m_aimCenter;
        [SerializeField] private Vector2 m_currentPosition;
        private bool m_aiming;

        private void Start()
        {
            m_anchorDistance = (m_anchor.position - m_camera.transform.position).magnitude;
        }

        private void Update()
        {
            if(Input.GetMouseButtonDown(0)) { m_aimCenter = GetMouseWorldPosition(); }
            if(Input.GetMouseButtonUp(0)) { Aim(m_aimCenter); }
            m_aiming = Input.GetMouseButton(0);
            
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
            m_target.position = m_anchor.position + targetOffset;
        }

        private Vector2 GetMouseWorldPosition()
            => m_camera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, m_anchorDistance));
    }
}
