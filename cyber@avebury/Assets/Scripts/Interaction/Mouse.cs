using UnityEngine;
using UnityEngine.Events;

namespace CyberAvebury
{
    [RequireComponent(typeof(Camera))]
    public abstract class Mouse : MonoBehaviour
    {
        private Camera m_camera;

        protected Camera Camera => m_camera;

        public UnityEvent<Vector2Int> OnMouseClicked;
        
        protected virtual void Awake()
        {
            m_camera = GetComponent<Camera>();
        }

        protected virtual void Update()
        {
            if(!Input.GetMouseButtonDown(0)) { return; }

            var mousePos = Input.mousePosition;
            Click(mousePos);
            OnMouseClicked?.Invoke(new Vector2Int((int) mousePos.x, (int) mousePos.y));

        }

        protected abstract void Click(Vector3 _mousePos);
    }
}
