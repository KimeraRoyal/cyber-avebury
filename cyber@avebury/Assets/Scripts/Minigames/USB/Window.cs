using UnityEngine;

namespace CyberAvebury
{
    [RequireComponent(typeof(Animator))]
    public class Window : MonoBehaviour
    {
        private static readonly int s_openVariable = Animator.StringToHash("Open");

        private Animator m_animator;
        
        private bool m_isOpen;

        public bool IsOpen
        {
            get => m_isOpen;
            set
            {
                if(m_isOpen == value) { return; }
                m_isOpen = value;
                m_animator.SetBool(s_openVariable, value);
            }
        }

        private void Awake()
        {
            m_animator = GetComponent<Animator>();
        }

        public void Open()
            => IsOpen = true;

        public void Close()
            => IsOpen = false;
    }
}
