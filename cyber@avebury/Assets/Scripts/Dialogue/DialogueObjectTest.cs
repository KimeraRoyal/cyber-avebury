using Sirenix.OdinInspector;
using UnityEngine;

namespace CyberAvebury
{
    public class DialogueObjectTest : MonoBehaviour
    {
        private Dialogue m_dialogue;

        [SerializeField] private DialogueLineObjectBase m_lines;

        private void Awake()
        {
            m_dialogue = FindAnyObjectByType<Dialogue>();
        }

        [Button(name: "Add Lines")]
        public void AddLines()
        {
            if(!m_dialogue) { return; }
            m_dialogue.AddLine(m_lines.GetLine());
        }
    }
}
