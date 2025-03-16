using UnityEngine;

namespace CyberAvebury
{
    [RequireComponent(typeof(RectTransform))]
    public class CopyMousePosition : MonoBehaviour
    {
        private RectTransform m_rect;

        private void Awake()
        {
            m_rect = GetComponent<RectTransform>();
        }

        private void Update()
        {
            m_rect.anchoredPosition = Input.mousePosition;
        }
    }
}
