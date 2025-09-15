using UnityEngine;
using UnityEngine.Events;

namespace CyberAvebury
{
    [RequireComponent(typeof(Camera))]
    public abstract class Mouse : MonoBehaviour
    {
        private InputHandling m_input;
        
        private Camera m_camera;

        private int m_lock;

        protected Camera Camera => m_camera;

        public bool Locked => m_lock > 0;
        
        public UnityEvent<Vector2Int> OnMouseClicked;

        public void Lock()
            => m_lock++;

        public void Unlock()
            => m_lock--;

        protected virtual void Awake()
        {
            m_input = FindAnyObjectByType<InputHandling>();
            m_input.OnPress.AddListener(OnMousePressed);
            
            m_camera = GetComponent<Camera>();
        }

        private void OnMousePressed(Vector2 _mousePosition)
        {
            if (Locked) { return; }
            var mousePos = _mousePosition;
            Cast(mousePos);
            OnMouseClicked?.Invoke(new Vector2Int((int) mousePos.x, (int) mousePos.y));
        }

        protected abstract void Cast(Vector3 _mousePos);
    }
}
