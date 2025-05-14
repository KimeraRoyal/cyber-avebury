using System;
using UnityEngine;
using UnityEngine.UI;

namespace CyberAvebury
{
    [RequireComponent(typeof(LayoutElement))]
    public class LayoutMatchRect : MonoBehaviour
    {
        private LayoutElement m_element;
        
        [SerializeField] private RectTransform m_target;

        private float m_previousHeight;

        private void Awake()
        {
            m_element = GetComponent<LayoutElement>();
        }

        private void Start()
        {
            ChangeHeight();
        }

        private void Update()
        {
            if(!m_target || Mathf.Abs(m_target.sizeDelta.y - m_previousHeight) < 0.001f) { return; }
            ChangeHeight();
        }

        private void ChangeHeight()
        {
            if(!m_target) { return; }
            m_element.minHeight = m_target.sizeDelta.y;
            m_previousHeight = m_element.minHeight;
        }
    }
}
