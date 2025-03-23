using KR;
using UnityEngine;

namespace CyberAvebury
{
    [RequireComponent(typeof(FrameAnimator))]
    public class DialoguePortrait : MonoBehaviour
    {
        private Dialogue m_dialogue;
        private WordWriter m_writer;

        private FrameAnimator m_animator;

        private void Awake()
        {
            m_dialogue = GetComponentInParent<Dialogue>();
            m_writer = m_dialogue.GetComponentInChildren<WordWriter>();

            m_animator = GetComponent<FrameAnimator>();
            
            m_writer.OnLineStarted.AddListener(SetIdlePortrait);
            m_writer.OnWordWritten.AddListener(SetTalkPortrait);
            m_writer.OnLineFinished.AddListener(SetIdlePortrait);
        }

        private void SetIdlePortrait(string _line)
        {
            var currentLine = m_dialogue.CurrentLine;
            m_animator.Animation = currentLine.Character.GetPortrait(currentLine.Expression, false);
        }

        private void SetTalkPortrait(string _word)
        {
            var currentLine = m_dialogue.CurrentLine;
            m_animator.Animation = currentLine.Character.GetPortrait(currentLine.Expression, true);
        }
    }
}