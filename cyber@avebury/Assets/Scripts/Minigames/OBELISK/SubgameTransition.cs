using System;
using System.Collections;
using UnityEngine;

namespace CyberAvebury
{
    [RequireComponent(typeof(Animator))]
    public class SubgameTransition : MonoBehaviour
    {
        public enum TransitionType
        {
            FromInterlude,
            ToInterlude,
            BetweenSubgames
        }
        
        private static readonly int s_transition = Animator.StringToHash("Transition");
        private static readonly int s_type = Animator.StringToHash("Type");

        private Animator m_animator;

        private bool m_triggerSceneChange;
        private Coroutine m_transitionCoroutine;

        private void Awake()
        {
            m_animator = GetComponent<Animator>();
        }

        public void BeginTransition(Action _changeScene, TransitionType _type)
        {
            if (m_transitionCoroutine != null)
            {
                Debug.LogWarning("Killed existing transition coroutine - was this intended?");
                StopCoroutine(m_transitionCoroutine);
            }
            m_animator.SetInteger(s_type, (int)_type);
            m_transitionCoroutine = StartCoroutine(WaitForTransition(_changeScene));
        }

        private IEnumerator WaitForTransition(Action _changeScene)
        {
            m_triggerSceneChange = false;
            m_animator.SetTrigger(s_transition);
            
            yield return new WaitUntil(() => m_triggerSceneChange);
            
            m_triggerSceneChange = false;
            _changeScene?.Invoke();
        }
        
        public void TriggerSceneChange()
        {
            m_triggerSceneChange = true;
        }
    }
}
