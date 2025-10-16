using System;
using UnityEngine;
using UnityEngine.UI;

namespace CyberAvebury
{
    [RequireComponent(typeof(Slider))]
    public class VolumeSlider : MonoBehaviour
    {
        private Slider m_slider;
        
        [SerializeField] private string m_channelPath;
        
        /*private VCA m_channel;

        private void Awake()
        {
            m_slider = GetComponent<Slider>();
        }

        private void Start()
        {
            m_channel = RuntimeManager.GetVCA(m_channelPath);

            m_channel.getVolume(out var volume);
            m_slider.value = volume;
            m_slider.onValueChanged.AddListener(OnValueChanged);
        }

        private void OnValueChanged(float _value)
        {
            m_channel.setVolume(m_slider.value);
        }*/
    }
}
