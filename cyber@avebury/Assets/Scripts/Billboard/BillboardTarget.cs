using UnityEngine;
using UnityEngine.Events;

namespace CyberAvebury
{
    public class BillboardTarget : MonoBehaviour
    {
        private Vector3 m_lastPosition;
        private Vector3 m_lastForward;

        public UnityEvent OnUpdated;

        private void Update()
        {
            if((transform.position - m_lastPosition).magnitude < 0.001f || (transform.forward - m_lastForward).magnitude < 0.001f) { return; }
            m_lastPosition = transform.position;
            m_lastForward = transform.forward;
            
            OnUpdated?.Invoke();
        }
    }
}
