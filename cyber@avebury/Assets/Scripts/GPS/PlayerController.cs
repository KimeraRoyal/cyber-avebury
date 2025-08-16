using UnityEngine;

namespace CyberAvebury
{
    public class PlayerController : MonoBehaviour
    {
        private GPS m_gps;

#if DEBUG
        [SerializeField] private LatLng m_debugPosition;

        [SerializeField] private float m_movementSpeed = 1.0f;
        [SerializeField] private float m_rotationSpeed = 1.0f;
#endif

        private void Awake()
        {
            m_gps = FindAnyObjectByType<GPS>();
        }

        private void Start()
        {
#if DEBUG
            transform.position = m_gps.GetScenePosition(m_debugPosition);
#endif
        }

        private void Update()
        {
            KeyboardMovement();
        }

        private void KeyboardMovement()
        {
#if DEBUG
            var movement = transform.forward * Input.GetAxis("Vertical");
            var rotation = Vector3.up * Input.GetAxis("Horizontal");
            transform.position += movement * (m_movementSpeed * Time.deltaTime);
            transform.eulerAngles += rotation * (m_rotationSpeed * Time.deltaTime);
#endif
        }
    }
}
