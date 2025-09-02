using Cinemachine;
using UnityEngine;

namespace CyberAvebury
{
    [RequireComponent(typeof(CinemachineVirtualCamera))]
    public class CameraGestures : MonoBehaviour
    {
        private CinemachineVirtualCamera m_camera;
        private CinemachineOrbitalTransposer m_transposer;
        
        [Range(0.0f, 1.0f)] [SerializeField] private float m_zoom = 0.5f;
        [SerializeField] private float m_zoomDamping = 1.0f;

        [SerializeField] private float m_scrollSpeed = 1.0f;

        [SerializeField] private AnimationCurve m_zoomYCurve = AnimationCurve.Linear(0.0f, 0.0f, 1.0f, 1.0f);
        [SerializeField] private AnimationCurve m_zoomZCurve = AnimationCurve.Linear(0.0f, 0.0f, 1.0f, 1.0f);

        private float m_currentZoom;
        private float m_zoomVelocity;

        private void Awake()
        {
            m_camera = GetComponent<CinemachineVirtualCamera>();
            m_transposer = m_camera.GetCinemachineComponent<CinemachineOrbitalTransposer>();
        }

        private void Update()
        {
            m_zoom = Mathf.Clamp01(m_zoom + Input.mouseScrollDelta.y * m_scrollSpeed);
            
            m_currentZoom = Mathf.SmoothDamp(m_currentZoom, m_zoom, ref m_zoomVelocity, m_zoomDamping);
            m_transposer.m_FollowOffset = new Vector3(0.0f, m_zoomYCurve.Evaluate(m_currentZoom), m_zoomZCurve.Evaluate(m_currentZoom));
        }
    }
}
