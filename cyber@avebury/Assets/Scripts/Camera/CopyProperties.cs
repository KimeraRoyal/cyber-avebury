using UnityEngine;

namespace CyberAvebury
{
    [RequireComponent(typeof(Camera))] [ExecuteInEditMode]
    public class CopyProperties : MonoBehaviour
    {
        private Camera m_camera;

        [SerializeField] private Camera m_target;

        private Camera Camera
        {
            get
            {
                if (!m_camera)
                {
                    m_camera = GetComponent<Camera>();
                }
                return m_camera;
            }
        }

        private void Update()
        {
            if (!Camera || !m_target) { return; }
            
            Camera.fieldOfView = m_target.fieldOfView;
        }
    }
}
