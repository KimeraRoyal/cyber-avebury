using Sirenix.OdinInspector;
using UnityEngine;

namespace CyberAvebury
{
    public class DialogueTest : MonoBehaviour
    {
        private Dialogue m_dialogue;

        [SerializeField] private DialogueLineContent[] m_lines;

        private void Awake()
        {
            m_dialogue = FindAnyObjectByType<Dialogue>();
        }

        [Button(name: "Test Lines")]
        private void TestLines()
        {
            if(!m_dialogue) { return; }
            m_dialogue.AddLine(new DialogueLines(m_lines));
        }
    }
}
