using System;
using UnityEngine;

namespace CyberAvebury
{
    [RequireComponent(typeof(Boss))]
    public class BossDialogue : MonoBehaviour
    {
        [Serializable]
        private class ProgressDialogue
        {
            [SerializeField] private float m_progress = 0.1f;
            private bool m_played;

            [SerializeField] private DialogueLineObjectBase m_dialogue;

            public DialogueLineBase GetDialogue(float _progress)
            {
                if (m_played || _progress < m_progress) { return null; }

                m_played = true;
                return m_dialogue.GetLine(); 
            }

            public void DeclarePlayed()
                => m_played = true;
        }
        
        private Dialogue m_dialogue;

        private Boss m_boss;

        [SerializeField] private ProgressDialogue[] m_dialogues;
        [SerializeField] private DialogueLineObjectBase m_defeatDialogue;

        private void Awake()
        {
            m_dialogue = FindAnyObjectByType<Dialogue>();
            
            m_boss = GetComponent<Boss>();
            
            m_boss.OnMove.AddListener(OnMove);
            
            m_boss.OnDefeated.AddListener(OnDefeated);
        }

        private void OnMove(float _progress)
        {
            foreach (var dialogue in m_dialogues)
            {
                var line = dialogue.GetDialogue(_progress);
                if(line == null) { return; }
                
                m_dialogue.AddLine(line);
                dialogue.DeclarePlayed();
            }
        }

        private void OnDefeated()
        {
            m_dialogue.AddLine(m_defeatDialogue.GetLine());
        }
    }
}