using CyberAvebury.Minigames;
using System;
using UnityEngine;

namespace CyberAvebury
{
    [RequireComponent(typeof(Minigame))]
    public class MinigameDialogue : MonoBehaviour
    {
        private Dialogue m_dialogue;

        private Minigame m_minigame;

        [SerializeField] private DialogueLineObjectBase m_beginDialogue;
        [SerializeField] private DialogueLineObjectBase m_passDialogue;
        [SerializeField] private DialogueLineObjectBase m_failDialogue;

        private bool m_paused;

        private void Awake()
        {
            m_dialogue = FindAnyObjectByType<Dialogue>();
            m_dialogue.OnNewDialogue.AddListener(OnNewDialogue);
            m_dialogue.OnEndDialogue.AddListener(OnEndDialogue);

            m_minigame = GetComponent<Minigame>();
            m_minigame.OnBegin.AddListener(OnBegin);
            m_minigame.OnPassed.AddListener(OnPass);
            m_minigame.OnFailed.AddListener(OnFail);
        }

        private void Start()
        {
            if(!m_dialogue.IsWriting) { return; }
            PauseMinigame();
        }

        private void OnDestroy()
        {
            m_dialogue.OnNewDialogue.RemoveListener(OnNewDialogue);
            m_dialogue.OnEndDialogue.RemoveListener(OnEndDialogue);
        }

        private void OnBegin()
        {
            if(m_beginDialogue == null) { return; }
            m_dialogue.AddLine(m_beginDialogue.GetLine());
        }

        private void OnPass()
        {
            if (m_passDialogue == null) { return; }
            m_dialogue.AddLine(m_passDialogue.GetLine());
        }

        private void OnFail()
        {
            if (m_failDialogue == null) { return; }
            m_dialogue.AddLine(m_failDialogue.GetLine());
        }

        private void OnNewDialogue(DialogueLineBase arg0)
            => PauseMinigame();

        private void OnEndDialogue()
            => UnpauseMinigame();

        private void PauseMinigame()
        {
            if(m_paused) {  return; }
            m_minigame.Pause();
            m_paused = true;
        }

        private void UnpauseMinigame()
        {
            if (!m_paused) { return; }
            m_minigame.Unpause();
            m_paused = false;
        }
    }
}
