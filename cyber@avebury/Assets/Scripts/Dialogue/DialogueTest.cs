using UnityEngine;

namespace CyberAvebury
{
    public class DialogueTest : MonoBehaviour
    {
        private Dialogue m_dialogue;

        [SerializeField] private DialogueCharacter m_character;

        private void Awake()
        {
            m_dialogue = FindAnyObjectByType<Dialogue>();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                m_dialogue.AddLine(m_character, 0, "My first test line. Isn't it neat?");
            }
            if (Input.GetKeyDown(KeyCode.B))
            {
                m_dialogue.AddLine(m_character, 0, "My second test line. It's just so cool!");
            }
            if (Input.GetKeyDown(KeyCode.C))
            {
                m_dialogue.AddLine(m_character, 0, "My third test line. I feel like I've seen this before.");
            }
            if (Input.GetKeyDown(KeyCode.D))
            {
                m_dialogue.AddLine(m_character, 0, "My fourth test line. Getting a bit sick of these, really.");
            }
            if (Input.GetKeyDown(KeyCode.E))
            {
                m_dialogue.AddLine(m_character, 0, "Well that's just gratuitous.");
            }
        }
    }
}
