using System;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace CyberAvebury
{
    [RequireComponent(typeof(CanvasGroup))]
    public class RangeOverlay : MonoBehaviour
    {
        private MapRange m_range;
        private PlayerController m_player;
        
        private CanvasGroup m_group;

        [SerializeField] private TMP_Text m_text;
        [TextArea(1, 5)] [SerializeField] private string m_gpsFailedText = "";
        [TextArea(1, 5)] [SerializeField] private string m_outOfRangeText = "";

        [SerializeField] private float m_fadeDuration = 1.0f;

        private Sequence m_fadeSequence;
        private bool m_wasValid;

        private void Awake()
        {
            m_range = FindAnyObjectByType<MapRange>();
            m_player = FindAnyObjectByType<PlayerController>();
            
            m_group = GetComponent<CanvasGroup>();
        }

        private void Update()
        {
            var valid = m_player.State switch
            {
                PlayerController.GPSState.Default => false,
                PlayerController.GPSState.Initialising => true,
                PlayerController.GPSState.Failed => false,
                PlayerController.GPSState.Active => true,
                PlayerController.GPSState.Disconnected => false,
                _ => throw new ArgumentOutOfRangeException()
            };
            if (valid)
            {
                valid = m_range.IsInRange(m_player.transform.position);
                if (!valid)
                {
                    m_text.text = string.Format(m_outOfRangeText, m_range.OutOfRangeBy(m_player.transform.position));
                }
            }
            else
            {
                m_text.text = m_gpsFailedText;
            }
            
            if(valid == m_wasValid) { return; }

            if (m_fadeSequence is { active: true })
            {
                m_fadeSequence.Kill();
            }

            m_fadeSequence = DOTween.Sequence();
            m_fadeSequence.Append(m_group.DOFade(valid ? 0.0f : 1.0f, m_fadeDuration));
            
            m_wasValid = valid;
        }
    }
}
