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
        [SerializeField] private AnimationCurve m_zoomRotationSpeedCurve = AnimationCurve.Linear(0.0f, 45.0f, 1.0f, 90.0f);
        [SerializeField] private AnimationCurve m_zoomMaxRotationSpeedCurve = AnimationCurve.Linear(0.0f, 30.0f, 1.0f, 45.0f);
        [SerializeField] private AnimationCurve m_zoomRotationDampingCurve = AnimationCurve.Linear(0.0f, 0.1f, 1.0f, 0.2f);

        [SerializeField] private float m_scrollSpeed = 1.0f;

        [SerializeField] private AnimationCurve m_zoomYCurve = AnimationCurve.Linear(0.0f, 0.0f, 1.0f, 1.0f);
        [SerializeField] private AnimationCurve m_zoomZCurve = AnimationCurve.Linear(0.0f, 0.0f, 1.0f, 1.0f);

        private float m_currentZoom;
        private float m_zoomVelocity;

        private float m_currentBias;
        private float m_biasVelocity;

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

            m_currentBias = Mathf.SmoothDamp(m_currentBias, 0.0f, ref m_biasVelocity, m_zoomRotationDampingCurve.Evaluate(m_zoom));
            var bias = m_transposer.m_Heading.m_Bias;
            bias += m_currentBias * m_zoomRotationSpeedCurve.Evaluate(m_zoom);
            if (bias < 180.0f) { bias += 360.0f; }
            if (bias > 180.0f) { bias -= 360.0f; }
            m_transposer.m_Heading.m_Bias = bias;
        }

        private void RotateCamera(Vector2 _delta)
        {
            var maxSpeed = m_zoomMaxRotationSpeedCurve.Evaluate(m_zoom);
            m_currentBias = Mathf.Clamp(m_currentBias + _delta.x, -maxSpeed, maxSpeed);
        }

        private void Zoom(float _delta)
        {
            m_zoom = Mathf.Clamp01(m_zoom + _delta * m_zoomSpeed);
        }
    }
}
