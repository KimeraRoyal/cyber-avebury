using System;
using CyberAvebury.Minigames;
using UnityEngine;

namespace CyberAvebury
{
    [RequireComponent(typeof(Animator))]
    public class USBBackground : MonoBehaviour
    {
        private static readonly int s_ejectVariable = Animator.StringToHash("Eject");

        private Minigame m_minigame;
        
        [SerializeField] private Window[] m_targets;

        private Animator m_animator;

        private void Awake()
        {
            m_minigame = GetComponentInParent<Minigame>();
            
            m_animator = GetComponent<Animator>();
        }

        public void OpenWindow(int _targetIndex)
            => m_targets[_targetIndex].Open();

        public void CloseWindow(int _targetIndex)
            => m_targets[_targetIndex].Close();

        public void EjectUSB()
            => m_animator.SetTrigger(s_ejectVariable);

        public void PassMinigame()
            => m_minigame.Pass();
    }
}
