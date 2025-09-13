using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CyberAvebury
{
    public class TimedDialogue : MonoBehaviour
    {
        [Serializable]
        public class TimedDialogueLines
        {
            [SerializeField] private float m_time;
            [SerializeField] private DialogueLineObjectBase m_line;

            private bool m_used;

            public float Time => m_time;
            public DialogueLineObjectBase Line => m_line;

            public bool Used => m_used;
            
            public void Use()
                => m_used = true;
        }

        private Dialogue m_dialogue;

        [SerializeField] private TimedDialogueLines[] m_lines;
        
        [SerializeField] [ReadOnly] private int m_counter;
        [SerializeField] [ReadOnly] private float m_timer;
        [SerializeField] [ReadOnly] private float m_nextTargetTime;
        [SerializeField] private bool m_paused;

        public bool Paused
        {
            get => m_paused;
            set => m_paused = value;
        }

        public void Pause() => Paused = true;
        public void Unpause() => Paused = false;

        private void Awake()
        {
            m_dialogue = FindAnyObjectByType<Dialogue>();
        }

        private void Start()
        {
            m_nextTargetTime = m_lines[0].Time;
        }

        private void Update()
        {
            if(m_paused || m_dialogue.HasDialogue || m_counter >= m_lines.Length) { return; }
            m_timer += Time.deltaTime;
            
            if(m_timer < m_nextTargetTime) { return; }

            if (!m_lines[m_counter].Used && m_lines[m_counter].Time > 0.001f)
            {
                m_dialogue.AddLine(m_lines[m_counter].Line.GetLine());
                m_lines[m_counter].Use();
            }
            m_counter++;
            
            if(m_counter >= m_lines.Length) { return; }
            m_nextTargetTime += m_lines[m_counter].Time;
        }
    }
}
