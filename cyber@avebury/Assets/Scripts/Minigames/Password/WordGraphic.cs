using TMPro;
using UnityEngine;

namespace CyberAvebury
{
    public class WordGraphic : MonoBehaviour
    {
        private Word m_word;
        
        private RectTransform m_rect;

        private TMP_Text m_labelText;

        [SerializeField] private float m_followTime = 0.1f;
        private Vector3 m_velocity;

        private float m_followTimeFactor = 1.0f;

        public Word Word
        {
            get => m_word;
            set
            {
                if (m_word)
                {
                    m_word.OnGrabbed.RemoveListener(OnWordGrabbed);
                    m_word.OnReleased.RemoveListener(OnWordReleased);
                }
                
                m_word = value;

                transform.position = m_word.transform.position;
                m_rect.sizeDelta = m_word.RectTransform.sizeDelta;
                m_labelText.text = m_word.Label;
                
                m_word.OnGrabbed.AddListener(OnWordGrabbed);
                m_word.OnReleased.AddListener(OnWordReleased);
            }
        }

        private void Awake()
        {
            m_rect = GetComponent<RectTransform>();

            m_labelText = GetComponentInChildren<TMP_Text>();
        }

        private void Update()
        {
            if(!m_word) { return; }

            var followTime = m_followTime * m_followTimeFactor;
            var position = Vector3.SmoothDamp(transform.position, m_word.transform.position, ref m_velocity, followTime);
            transform.position = position;
        }

        private void OnWordGrabbed()
        {
            m_followTimeFactor = 0.0f;
        }

        private void OnWordReleased()
        {
            m_followTimeFactor = 1.0f;
        }
    }
}
