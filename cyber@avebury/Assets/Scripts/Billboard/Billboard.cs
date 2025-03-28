using UnityEngine;

namespace CyberAvebury
{
    public class Billboard : MonoBehaviour
    {
        [SerializeField] private BillboardTarget m_target;

        private Vector3 m_offset;

        private void Awake()
        {
            m_offset = transform.localEulerAngles;
            
            if(m_target) { return; }

            m_target = FindAnyObjectByType<BillboardTarget>();
        }

        private void OnEnable()
        {
            m_target.OnUpdated.AddListener(OnTargetUpdated);
        }

        private void OnDisable()
        {
            m_target.OnUpdated.RemoveListener(OnTargetUpdated);
        }

        private void OnTargetUpdated()
        {
            transform.LookAt(m_target.transform);
            transform.Rotate(m_offset);
        }
    }
}
