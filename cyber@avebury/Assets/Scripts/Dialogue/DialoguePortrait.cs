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
            var currentDialogue = m_dialogue.CurrentDialogue;
            var expression = currentDialogue.GetExpressionIndex(m_dialogue.CurrentLineIndex);
            
            m_animator.Animation = currentDialogue.Character.GetPortrait(expression, false);
        }

        private void SetTalkPortrait(string _word)
        {
            var currentDialogue = m_dialogue.CurrentDialogue;
            var expression = currentDialogue.GetExpressionIndex(m_dialogue.CurrentLineIndex);
            
            m_animator.Animation = currentDialogue.Character.GetPortrait(expression, true);
        }
    }
}