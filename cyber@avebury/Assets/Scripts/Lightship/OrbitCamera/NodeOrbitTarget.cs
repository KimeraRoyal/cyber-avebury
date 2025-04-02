using Niantic.Lightship.Maps.SampleAssets.Cameras.OrbitCamera;
using UnityEngine;

namespace CyberAvebury
{
    [RequireComponent(typeof(OrbitCameraController))]
    public class NodeOrbitTarget : MonoBehaviour
    {
        private NodeSelection m_selection;

        private OrbitCameraController m_controller;

        private void Awake()
        {
            m_selection = FindAnyObjectByType<NodeSelection>();

            m_controller = GetComponent<OrbitCameraController>();
            
            m_selection.OnNodeSelected.AddListener(OnNodeSelected);
        }

        private void OnNodeSelected(Node _node)
            => m_controller.SetFocus(_node ? _node.transform : null);
    }
}
