using System;
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

        public UnityEvent<float> OnSwipe;
        public UnityEvent<float> OnZoom;

        private void Start()
        {
            m_swipeGesture.Transformed += Swipe;
            m_zoomGesture.Transformed += Zoom;
        }

        private void Swipe(object _sender, EventArgs _args)
        {
            OnSwipe?.Invoke(m_swipeGesture.DeltaPosition.x / Screen.width);
        }

        private void Zoom(object _sender, EventArgs _args)
        {
            OnZoom?.Invoke(m_zoomGesture.DeltaScale - 1.0f);
        }
    }
}
