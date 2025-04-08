using CyberAvebury.Minigames.Timer;
using UnityEngine;

namespace CyberAvebury
{
    [RequireComponent(typeof(RectTransform))]
    public class TimerFill : MonoBehaviour
    {
        private MinigameTimer m_timer;
        
        private RectTransform m_rectTransform;

        private void Awake()
        {
            m_timer = GetComponentInParent<MinigameTimer>();
            
            m_rectTransform = GetComponent<RectTransform>();
            
            m_timer.OnTimerUpdated.AddListener(OnTimerUpdated);
        }

        private void Start()
            => UpdateFill(0);

        private void OnTimerUpdated(float _time)
            => UpdateFill(m_timer.TimerProgress);

        private void UpdateFill(float _t)
        {
            var anchorMax = m_rectTransform.anchorMax;
            anchorMax.x = 1 - _t;
            m_rectTransform.anchorMax = anchorMax;

            m_rectTransform.offsetMin = Vector2.zero;
        }
    }
}
