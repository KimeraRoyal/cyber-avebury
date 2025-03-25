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

        [SerializeField] private FrameAnimation m_static;

        private void Awake()
        {
            m_dialogue = GetComponentInParent<Dialogue>();
            m_writer = m_dialogue.GetComponentInChildren<WordWriter>();

            m_animator = GetComponent<FrameAnimator>();
            
            m_dialogue.OnNewDialogue.AddListener(_ => SetStaticPortrait());
            m_dialogue.OnEndDialogue.AddListener(SetStaticPortrait);
            
            m_writer.OnLineStarted.AddListener(_ => SetStaticPortrait());
            m_writer.OnWordWritten.AddListener(SetTalkPortrait);
            m_writer.OnLineFinished.AddListener(SetIdlePortrait);
        }

        private void SetStaticPortrait()
            => m_animator.Animation = m_static;

        private void SetIdlePortrait(string _line)
        {
            var currentLine = m_dialogue.CurrentDialogue.GetContent(m_dialogue.CurrentLineIndex);
            var expression = currentLine.Expression;
            
            m_animator.Animation = currentLine.Character.GetPortrait(expression, false);
        }

        private void SetTalkPortrait(string _word)
        {
            var currentLine = m_dialogue.CurrentDialogue.GetContent(m_dialogue.CurrentLineIndex);
            var expression = currentLine.Expression;
            
            m_animator.Animation = currentLine.Character.GetPortrait(expression, true);
        }
    }
}