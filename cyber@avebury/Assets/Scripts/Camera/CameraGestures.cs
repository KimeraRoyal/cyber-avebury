using Cinemachine;
using UnityEngine;

namespace CyberAvebury
{
    [RequireComponent(typeof(CinemachineVirtualCamera))]
    public class CameraGestures : MonoBehaviour
    {
        private InputHandling m_inputHandling;
        
        private CinemachineVirtualCamera m_camera;
        private CinemachineOrbitalTransposer m_transposer;
        
        [Range(0.0f, 1.0f)] [SerializeField] private float m_zoom = 0.5f;
        [SerializeField] private float m_zoomSpeed = 1.0f;
        [SerializeField] private float m_zoomDamping = 1.0f;

        [Range(-180.0f, 180.0f)] [SerializeField] private float m_rotation;
        [SerializeField] private float m_rotateSpeed = 180.0f;

        [SerializeField] private float m_scrollSpeed = 1.0f;

        [SerializeField] private AnimationCurve m_zoomYCurve = AnimationCurve.Linear(0.0f, 0.0f, 1.0f, 1.0f);
        [SerializeField] private AnimationCurve m_zoomZCurve = AnimationCurve.Linear(0.0f, 0.0f, 1.0f, 1.0f);

        private float m_currentZoom;
        private float m_zoomVelocity;

        private void Awake()
        {
            m_inputHandling = FindAnyObjectByType<InputHandling>();
            m_inputHandling.OnSwipe.AddListener(RotateCamera);
            m_inputHandling.OnZoom.AddListener(Zoom);
            
            m_camera = GetComponent<CinemachineVirtualCamera>();
            m_transposer = m_camera.GetCinemachineComponent<CinemachineOrbitalTransposer>();
        }

        private void Update()
        {
            m_zoom = Mathf.Clamp01(m_zoom + Input.mouseScrollDelta.y * m_scrollSpeed);
            
            m_currentZoom = Mathf.SmoothDamp(m_currentZoom, m_zoom, ref m_zoomVelocity, m_zoomDamping);
            m_transposer.m_FollowOffset = new Vector3(0.0f, m_zoomYCurve.Evaluate(m_currentZoom), m_zoomZCurve.Evaluate(m_currentZoom));
        }

        private void RotateCamera(float _delta)
        {
            var bias = m_transposer.m_Heading.m_Bias;
            bias += _delta * m_rotateSpeed;
            if (bias < 180.0f) { bias += 360.0f; }
            if (bias > 180.0f) { bias -= 360.0f; }
            m_transposer.m_Heading.m_Bias = bias;
        }

        private void Zoom(float _delta)
        {
            m_zoom = Mathf.Clamp01(m_zoom + _delta * m_zoomSpeed);
        }
    }
}
