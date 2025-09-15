using System;
using System.Linq;
using Sirenix.OdinInspector;
using TouchScript;
using TouchScript.Gestures.TransformGestures;
using UnityEngine;
using UnityEngine.Events;

namespace CyberAvebury
{
    public class InputHandling : MonoBehaviour
    {
        [SerializeField] private ScreenTransformGesture m_swipeGesture;
        [SerializeField] private ScreenTransformGesture m_zoomGesture;

        [SerializeField] private float m_swipeSpeed = 1.0f;

        private int m_pointerId = -1;
        [ReadOnly] [SerializeField] private Vector2 m_pointerPosition;

        public Vector2 PointerPosition => m_pointerPosition;

        public UnityEvent<Vector2> OnPress;
        public UnityEvent<Vector2> OnUnpress;
        public UnityEvent<Vector2> OnSwipe;
        public UnityEvent<float> OnZoom;

        private void OnEnable()
        {
            if (TouchManager.Instance != null)
            {
                TouchManager.Instance.PointersPressed += PointersPressed;
                TouchManager.Instance.PointersReleased += PointersReleased;
            }
            m_swipeGesture.Transformed += Swipe;
            m_zoomGesture.Transformed += Zoom;
        }

        private void OnDisable()
        {
            if (TouchManager.Instance != null)
            {
                TouchManager.Instance.PointersPressed -= PointersPressed;
            }
            m_swipeGesture.Transformed -= Swipe;
            m_zoomGesture.Transformed -= Zoom;
        }

        private void PointersPressed(object _sender, PointerEventArgs _e)
        {
            if(_e.Pointers.Count != 1) { return; }

            m_pointerId = _e.Pointers[0].Id;
            m_pointerPosition = _e.Pointers[0].Position;
            OnPress?.Invoke(m_pointerPosition);
        }

        private void PointersReleased(object _sender, PointerEventArgs _e)
        {
            var pointer = _e.Pointers.FirstOrDefault(_pointer => _pointer.Id == m_pointerId);
            if(pointer == null) { return; }

            m_pointerPosition = pointer.Position;
            OnUnpress?.Invoke(m_pointerPosition);
            m_pointerId = -1;
        }

        private void Swipe(object _sender, EventArgs _args)
        {
            m_pointerPosition += new Vector2(m_swipeGesture.DeltaPosition.x, m_swipeGesture.DeltaPosition.y);
            OnSwipe?.Invoke(new Vector2(m_swipeGesture.DeltaPosition.x / Screen.width, m_swipeGesture.DeltaPosition.y / Screen.height));
        }

        private void Zoom(object _sender, EventArgs _args)
        {
            OnZoom?.Invoke(m_zoomGesture.DeltaScale - 1.0f);
        }
    }
}
