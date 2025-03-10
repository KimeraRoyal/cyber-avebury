using UnityEngine;

namespace CyberAvebury
{
    public class CopyRotation : MonoBehaviour
    {
        [SerializeField] private Transform m_target;

        private void Update()
        {
            transform.rotation = m_target.rotation;
        }
    }
}
