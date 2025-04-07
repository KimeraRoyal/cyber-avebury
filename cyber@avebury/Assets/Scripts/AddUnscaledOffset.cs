using UnityEngine;
using UnityEngine.UI;

namespace CyberAvebury
{
    [RequireComponent(typeof(RectTransform))]
    public class AddUnscaledOffset : MonoBehaviour
    {
        private CanvasScaler m_scaler;
        
        private RectTransform m_rect;

        [SerializeField] private Vector2 m_offsetMin;
        [SerializeField] private Vector2 m_offsetMax;

        private Vector2 m_startingMin;
        private Vector2 m_startingMax;
        private float m_lastScaleFactor = -1;

        private void Awake()
        {
            m_scaler = GetComponentInParent<CanvasScaler>();

            m_rect = GetComponent<RectTransform>();
        }

        private void Start()
        {
            m_startingMin = m_rect.offsetMin;
            m_startingMax = m_rect.offsetMax;
            
            UpdateScale();
        }

        private void Update()
        {
            UpdateScale();
        }

        private void UpdateScale()
        {
            if(Mathf.Abs(m_scaler.scaleFactor - m_lastScaleFactor) < 0.0001f) { return; }
            m_lastScaleFactor = m_scaler.scaleFactor;

            var scaledOffsetMin = m_offsetMin / m_scaler.scaleFactor;
            var scaledOffsetMax = m_offsetMax / m_scaler.scaleFactor;

            m_rect.offsetMin = m_startingMin + scaledOffsetMin;
            m_rect.offsetMax = m_startingMax + scaledOffsetMax;
        }
    }
}
