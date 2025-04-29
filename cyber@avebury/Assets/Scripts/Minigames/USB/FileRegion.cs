using System;
using UnityEngine;
using UnityEngine.Events;

namespace CyberAvebury
{
    public class FileRegion : MonoBehaviour
    {
        private RectTransform m_rect;
        
        public UnityEvent<File> OnFileAdded;

        private void Awake()
        {
            m_rect = GetComponent<RectTransform>();
        }

        public void AddFile(File _file)
        {
            OnFileAdded?.Invoke(_file);
        }

        public void ClampRect(RectTransform _other)
        {
            var boundsMin = m_rect.anchoredPosition + m_rect.rect.min;
            var boundsMax = m_rect.anchoredPosition + m_rect.rect.max;
            
            var min = _other.anchoredPosition + _other.rect.min;
            var max = _other.anchoredPosition + _other.rect.max;

            var distanceMin = boundsMin - min;
            var distanceMax = boundsMax - max;
            for (var i = 0; i < 2; i++)
            {
                distanceMin[i] = Mathf.Max(0, distanceMin[i]);
                distanceMax[i] = Mathf.Min(0, distanceMax[i]);
            }

            var offset = distanceMin + distanceMax;
            _other.anchoredPosition += offset;
        }
    }
}
