using UnityEngine;

namespace CyberAvebury
{
    [RequireComponent(typeof(Animator))]
    public class DialogueAnimator : MonoBehaviour
    {
        private static int s_showVariable = Animator.StringToHash("Show");

        private Dialogue m_dialogue;
        
        private Animator m_animator;

        private void Awake()
        {
            m_dialogue = GetComponentInParent<Dialogue>();
            
            m_animator = GetComponent<Animator>();
            
            m_dialogue.OnNewDialogue.AddListener(OnNewDialogue);
            m_dialogue.OnEndDialogue.AddListener(OnEndDialogue);
        }

        private void OnNewDialogue(DialogueLineBase _dialogue)
        {
            m_animator.SetBool(s_showVariable, true);
        }

        private void OnEndDialogue()
        {
            m_animator.SetBool(s_showVariable, false);
        }
    }
}
