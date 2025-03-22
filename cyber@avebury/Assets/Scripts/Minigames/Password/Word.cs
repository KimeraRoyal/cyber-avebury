using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace CyberAvebury
{
    [RequireComponent(typeof(RectTransform))]
    public class Word : MonoBehaviour
    {
        private RectTransform m_rect;

        private TMP_Text m_labelText;
        
        private DummyWord m_dummy;
        private DummyWord m_passwordDummy;

        public RectTransform RectTransform => m_rect;

        public string Label
        {
            get => m_labelText.text;
            set => m_labelText.text = value;
        }

        public DummyWord Dummy
        {
            get => m_dummy;
            set => m_dummy = value;
        }

        public DummyWord PasswordDummy
        {
            get => m_passwordDummy;
            set => m_passwordDummy = value;
        }

        public UnityEvent OnGrabbed;
        public UnityEvent OnReleased;
        
        private void Awake()
        {
            m_rect = GetComponent<RectTransform>();

            m_labelText = GetComponentInChildren<TMP_Text>();
        }

        public void Grab()
        {
            OnGrabbed?.Invoke();
        }

        public void Release()
        {
            OnReleased?.Invoke();
        }
    }
}
