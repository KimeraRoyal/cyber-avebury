using Cinemachine;
using UnityEngine;

namespace CyberAvebury
{
    [RequireComponent(typeof(CinemachineVirtualCamera))]
    public class PlayerMatchedVirtualCamera : MonoBehaviour
    {
        private Player m_player;
        
        private CinemachineVirtualCamera m_camera;
        private CinemachineOrbitalTransposer m_orbitalTransposer;

        private void Awake()
        {
            m_player = FindAnyObjectByType<Player>();

            m_camera = GetComponent<CinemachineVirtualCamera>();
            m_orbitalTransposer = m_camera.GetCinemachineComponent<CinemachineOrbitalTransposer>();
        }

        private void OnEnable()
        {
            m_orbitalTransposer.m_Heading.m_Bias = m_player.transform.eulerAngles.y;
        }
    }
}
