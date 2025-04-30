using UnityEngine;

namespace CyberAvebury
{
    public class KeepUpright : MonoBehaviour
    {
        private void LateUpdate()
        {
            transform.eulerAngles = Vector3.zero;
        }
    }
}
