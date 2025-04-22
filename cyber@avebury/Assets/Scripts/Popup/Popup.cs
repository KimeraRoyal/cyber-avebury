using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace CyberAvebury
{
    [RequireComponent(typeof(Animator))]
    public class Popup : MonoBehaviour
    {
        private Dialogue m_dialogue;

        private static readonly int c_showVariable = Animator.StringToHash("Show");
        private Animator m_animator;

        private PopupInfo m_currentPopup;

        private bool m_willShow;

        public PopupInfo CurrentPopup => m_currentPopup;

        public UnityEvent<PopupInfo> OnPopupShown;
        
        private void Awake()
        {
            m_dialogue = FindAnyObjectByType<Dialogue>();

            m_animator = GetComponent<Animator>();
        }

        public void Show(PopupInfo _info)
        {
            if(m_willShow) { return; }
            m_willShow = true;
            StartCoroutine(ShowAfterLoadingScreen(_info));
        }

        private IEnumerator ShowAfterLoadingScreen(PopupInfo _info)
        {
            yield return new WaitUntil(() => !LoadingScreen.Instance.IsOpened);
            if (!m_willShow) { yield break; }

            m_animator.SetBool(c_showVariable, true);

            m_currentPopup = _info;
            OnPopupShown?.Invoke(_info);
        }

        public void Hide()
        {
            m_dialogue.AddLine(m_currentPopup.OnClosedDialogue.GetLine());

            m_animator.SetBool(c_showVariable, false);

            m_willShow = false;
            m_currentPopup = null;
        }
    }
}
