using System;
using UnityEngine;
using UnityEngine.Events;

namespace CyberAvebury
{
    [RequireComponent(typeof(Animator))]
    public class Popup : MonoBehaviour
    {
        private static readonly int c_showVariable = Animator.StringToHash("Show");
        private Animator m_animator;

        private PopupInfo m_currentPopup;

        public PopupInfo CurrentPopup => m_currentPopup;

        public UnityEvent<PopupInfo> OnPopupShown;
        
        private void Awake()
        {
            m_animator = GetComponent<Animator>();
        }

        public void Show(PopupInfo _info)
        {
            m_animator.SetBool(c_showVariable, true);

            m_currentPopup = _info;
            OnPopupShown?.Invoke(_info);
        }

        public void Hide()
        {
            m_animator.SetBool(c_showVariable, false);

            m_currentPopup = null;
        }
    }
}
