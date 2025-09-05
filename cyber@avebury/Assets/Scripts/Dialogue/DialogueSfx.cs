using System;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;
using STOP_MODE = FMOD.Studio.STOP_MODE;

namespace CyberAvebury
{
    public class DialogueSfx : MonoBehaviour
    {
        private Dialogue m_dialogue;
        private WordWriter m_writer;
        
        [SerializeField] private EventReference m_noiseSfx;

        private EventInstance m_noiseSfxInstance;
        private PARAMETER_ID m_noiseFadeId;

        private DialogueCharacter m_currentCharacter;

        private void Awake()
        {
            m_dialogue = GetComponentInParent<Dialogue>();
            m_writer = m_dialogue.GetComponentInChildren<WordWriter>();

            m_dialogue.OnNewDialogue.AddListener(_ => NewDialogue());
            m_dialogue.OnEndDialogue.AddListener(EndDialogue);
            
            m_writer.OnLineStarted.AddListener(_ => OnLineStarted());
            m_writer.OnWordWritten.AddListener(_ => OnWordWritten());
            m_writer.OnLineFinished.AddListener(_ => StopStatic());
        }

        private void Start()
        {
            var noiseSfxDescription = RuntimeManager.GetEventDescription(m_noiseSfx);
            noiseSfxDescription.getParameterDescriptionByName("Fade Out Static", out var noiseFadeDescription);
            m_noiseFadeId = noiseFadeDescription.id;

            m_noiseSfxInstance = RuntimeManager.CreateInstance(m_noiseSfx);
        }

        private void OnDestroy()
        {
            m_noiseSfxInstance.stop(STOP_MODE.IMMEDIATE);
            m_noiseSfxInstance.release();
        }

        private void NewDialogue()
        {
            m_noiseSfxInstance.setParameterByID(m_noiseFadeId, 0);
            PlayStatic();
        }

        private void EndDialogue()
        {
            m_noiseSfxInstance.setParameterByID(m_noiseFadeId, 1);
            PlayStatic();
        }

        private void OnLineStarted()
        {
            m_currentCharacter = m_dialogue.CurrentDialogue.GetCharacter(m_dialogue.CurrentLineIndex);
            PlayStatic();
        }

        private void OnWordWritten()
        {
            m_currentCharacter.PlayVoiceSfx();
            StopStatic();
        }

        private void PlayStatic()
        {
            m_noiseSfxInstance.start();
        }

        private void StopStatic()
        {
            m_noiseSfxInstance.stop(STOP_MODE.ALLOWFADEOUT);
        }
    }
}
