using System;
using FMOD.Studio;
using FMODUnity;
using Sirenix.OdinInspector;
using UnityEngine;
using STOP_MODE = FMOD.Studio.STOP_MODE;

namespace CyberAvebury
{
    public class MusicPlayer : MonoBehaviour
    {
        public enum State
        {
            None,
            Title,
            Tutorial,
            Overworld,
            Trouble,
            Obelisk,
            Boss,
            USB,
            Spinning
        }

        [Serializable]
        private class Track
        {
            [SerializeField] private EventReference m_event;
            
            private EventInstance m_instance;

            public EventReference Event => m_event;
            public EventInstance Instance
            {
                get
                {
                    if (!m_instance.hasHandle())
                    {
                        m_instance = RuntimeManager.CreateInstance(m_event);
                    }
                    return m_instance;
                }
            }
        }
        
        private static MusicPlayer s_instance;
        public static MusicPlayer Instance
        {
            get
            {
                if(!s_instance) { s_instance = FindAnyObjectByType<MusicPlayer>(); }
                return s_instance;
            }
            private set => s_instance = value;
        }

        [OnValueChanged("OnNewState")] [EnumToggleButtons]
        [SerializeField] private State m_state;

        [SerializeField] private Track m_titleTheme;
        private PARAMETER_DESCRIPTION m_connectedState;
        
        [SerializeField] private Track m_overworldTheme;
        [SerializeField] private Track m_troubleTheme;
        [SerializeField] private Track m_obeliskTheme;
        [SerializeField] private Track m_bossTheme;
        
        [SerializeField] private Track m_usbTheme;
        private PARAMETER_DESCRIPTION m_spinning;

        private State m_currentState;
        private EventInstance m_currentSong;

        private void Awake()
        {
            if (Instance && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            DontDestroyOnLoad(gameObject);
            Instance = this;
            
            m_connectedState = GetParameterDescription(m_titleTheme.Event, "Connected State");
            m_spinning = GetParameterDescription(m_usbTheme.Event, "Spinning");
        }

        private void OnNewState()
        {
            if (m_state == State.None)
            { 
                StopSong();
                return;
            }
            ChangeMusicState(m_state);
        }

        public void ChangeMusicState(State _state, bool _restartIfSame = false)
        {
            if(_state == State.None) { return; }
            
            Debug.Log($"[{gameObject.name}]: {m_currentState} -> {_state}");
            
            if (_state == m_currentState)
            {
                if(!_restartIfSame) { return; }

                m_currentSong.start();
                return;
            }

            if (!(_state == State.Tutorial && m_currentState == State.Title) && !(_state == State.Spinning && m_currentState == State.USB))
            {
                PlaySong(GetSongFromState(_state));
            }
            if (_state is State.Title or State.Tutorial)
            {
                m_currentSong.setParameterByID(m_connectedState.id, _state == State.Tutorial ? 1.0f : 0.0f);
            }
            if (_state is State.USB or State.Spinning)
            {
                m_currentSong.setParameterByID(m_spinning.id, _state == State.Spinning ? 1.0f : 0.0f);
            }

            m_currentState = _state;
            m_state = _state;
        }

        private void PlaySong(EventInstance _song)
        {
            if(!_song.hasHandle()) { return; }
            
            if (m_currentSong.hasHandle())
            {
                StopSong();
            }
            m_currentSong = _song;
            m_currentSong.start();
        }

        public void StopSong()
        {
            if(!m_currentSong.hasHandle()) { return; }

            m_currentSong.stop(STOP_MODE.ALLOWFADEOUT);
            m_currentState = State.None;
        }

        private EventInstance GetSongFromState(State _state)
        {
            var track = _state switch
            {
                State.Title or State.Tutorial => m_titleTheme,
                State.Overworld => m_overworldTheme,
                State.Trouble => m_troubleTheme,
                State.Obelisk => m_obeliskTheme,
                State.Boss => m_bossTheme,
                State.USB => m_usbTheme,
                _ => null
            };
            return track?.Instance ?? new EventInstance();
        }

        private static PARAMETER_DESCRIPTION GetParameterDescription(EventReference _event, string _parameterName)
        {
            var description = RuntimeManager.GetEventDescription(_event);
            var result = description.getParameterDescriptionByName(_parameterName, out var parameter);
            return parameter;
        }
    }
}
