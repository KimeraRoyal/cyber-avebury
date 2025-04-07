using System;
using UnityEngine;

namespace CyberAvebury
{
    [RequireComponent(typeof(Animator))]
    public class MenuScreens : MonoBehaviour
    {
        private static readonly int s_stateVariable = Animator.StringToHash("State");

        public enum MenuState
        {
            MainMenu,
            Credits
        }

        private Animator m_animator;

        private MenuState m_currentState;

        public MenuState CurrentState => m_currentState;

        private void Awake()
        {
            m_animator = GetComponent<Animator>();
        }

        public void ChangeState(MenuState _nextState)
        {
            m_currentState = _nextState;
            m_animator.SetInteger(s_stateVariable, (int) m_currentState);
        }

        public void ChangeState(int _nextStateIndex)
        {
            if(_nextStateIndex < 0 || _nextStateIndex >= Enum.GetValues(typeof(MenuState)).Length) { return; }
            ChangeState((MenuState) _nextStateIndex);
        }
    }
}
