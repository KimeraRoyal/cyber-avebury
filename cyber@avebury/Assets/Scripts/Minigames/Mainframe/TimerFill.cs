using System;
using CyberAvebury.Minigames.Mainframe;
using UnityEngine;

namespace CyberAvebury
{
    [RequireComponent(typeof(RectTransform))]
    public class TimerFill : MonoBehaviour
    {
        private Mainframe m_mainframe;
        
        private RectTransform m_rectTransform;

        private void Awake()
        {
            m_mainframe = GetComponentInParent<Mainframe>();
            
            m_rectTransform = GetComponent<RectTransform>();
            
            m_mainframe.OnTimerUpdated.AddListener(OnTimerUpdated);
        }

        private void Start()
            => UpdateFill(0);

        private void OnTimerUpdated(float _time)
            => UpdateFill(m_mainframe.TimerProgress);

        private void UpdateFill(float _t)
        {
            var anchorMax = m_rectTransform.anchorMax;
            anchorMax.x = 1 - _t;
            m_rectTransform.anchorMax = anchorMax;

            m_rectTransform.offsetMin = Vector2.zero;
        }
    }
}
