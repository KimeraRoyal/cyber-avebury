using System;
using System.Collections;
using UnityEngine;

namespace CyberAvebury
{
    public class Flash : MonoBehaviour
    {
        private static readonly int s_flash = Animator.StringToHash("Flash");

        private Dialogue m_dialogue;
        
        private Animator m_animator;

        private bool m_triggerSceneChange;
        private Coroutine m_transitionCoroutine;

        private void Awake()
        {
            m_dialogue = FindAnyObjectByType<Dialogue>();
            
            m_animator = GetComponent<Animator>();
        }

        public void BeginFlash(Action _changeScene)
        {
            if (m_transitionCoroutine != null)
            {
                Debug.LogWarning("Killed existing flash coroutine - was this intended?");
                StopCoroutine(m_transitionCoroutine);
            }
            m_dialogue.Pause = true;
            m_transitionCoroutine = StartCoroutine(WaitForFlash(_changeScene));
        }

        private IEnumerator WaitForFlash(Action _changeScene)
        {
            m_triggerSceneChange = false;
            m_animator.SetTrigger(s_flash);
            
            yield return new WaitUntil(() => m_triggerSceneChange);
            
            m_triggerSceneChange = false;
            _changeScene?.Invoke();
            m_dialogue.Pause = false;
        }
        
        public void TriggerSceneChange()
        {
            m_triggerSceneChange = true;
        }
    }
}