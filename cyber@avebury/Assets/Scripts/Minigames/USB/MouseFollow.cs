using UnityEngine;

namespace CyberAvebury
{
    public class MouseFollow : MonoBehaviour
    {
        private InputHandling m_input;
        
        private Canvas m_canvas;

        private RectTransform m_rect;

        private void Awake()
        {
            m_input = FindAnyObjectByType<InputHandling>();

            m_canvas = GetComponentInParent<Canvas>();

            m_rect = GetComponent<RectTransform>();
        }

        private void Update()
        {
            m_rect.anchoredPosition = m_input.PointerPosition / m_canvas.scaleFactor;
        }
    }
}
