using System;
using UnityEngine;
using UnityEngine.Events;

namespace CyberAvebury
{
    [RequireComponent(typeof(Camera))]
    public class Mouse : MonoBehaviour
    {
        private Camera m_camera;

        public UnityEvent<Vector2Int> OnMouseClicked;
        public UnityEvent<Vector2> OnMouseClickedWorld;
        
        private void Awake()
        {
            m_camera = GetComponent<Camera>();
        }

        private void Update()
        {
            if(!Input.GetMouseButtonDown(0)) { return; }

            var mousePos = Input.mousePosition;
            OnMouseClicked?.Invoke(new Vector2Int((int) mousePos.x, (int) mousePos.y));

            var worldPos = m_camera.ScreenToWorldPoint(mousePos);
            OnMouseClickedWorld?.Invoke(worldPos);
        }
    }
}
