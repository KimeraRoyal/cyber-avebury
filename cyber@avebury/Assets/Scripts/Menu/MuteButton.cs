using System;
using UnityEngine;
using UnityEngine.UI;

namespace CyberAvebury
{
    [RequireComponent(typeof(Toggle))]
    public class MuteButton : MonoBehaviour
    {
        private MuteAudio m_mute;
        
        private Toggle m_toggle;

        private void Awake()
        {
            m_mute = FindAnyObjectByType<MuteAudio>();
            
            m_toggle = GetComponent<Toggle>();
            
            m_mute.OnMuteChanged += OnMuteChanged;
        }

        private void Start()
        {
            m_toggle.onValueChanged.AddListener(OnToggled);
        }

        private void OnEnable()
        {
            m_toggle.isOn = !m_mute.IsMuted;
        }

        private void OnMuteChanged(bool _muted)
        {
            if(m_toggle.isOn == !_muted) { return; }
            m_toggle.isOn = !_muted;
        }

        private void OnToggled(bool _value)
        {
            if(_value == !m_mute.IsMuted) { return; }
            m_mute.IsMuted = !_value;
            m_toggle.isOn = !m_mute.IsMuted;
        }
    }
}
