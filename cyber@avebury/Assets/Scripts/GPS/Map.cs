using UnityEngine;

namespace CyberAvebury
{
    public class Map : MonoBehaviour
    {
        private GPS m_gps;

        private void Start()
        {
            transform.position = m_gps.GetScenePosition(m_gps.Origin);
        }
    }
}
