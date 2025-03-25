using UnityEngine;

namespace CyberAvebury
{
    public class DialogueTest : MonoBehaviour
    {
        private Dialogue m_dialogue;

        [SerializeField] private DialogueCharacter m_character1;
        [SerializeField] private DialogueCharacter m_character2;

        private void Awake()
        {
            m_dialogue = FindAnyObjectByType<Dialogue>();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                var lines = new DialogueLineContent[4];
                lines[0] = new DialogueLineContent(m_character1, 0, "You know, it's really pleasant here. Warm sun, clear sky, green grass...");
                lines[1] = new DialogueLineContent(m_character1, 0, "Really makes you feel all nice inside, haha. You know?");
                lines[2] = new DialogueLineContent(m_character2, 0, "I'M GONNA BLOW THIS WHOLE PLACE UP. WITH A BOMB!");
                lines[3] = new DialogueLineContent(m_character1, 1, "...");
                m_dialogue.AddLine(new DialogueLines(lines));
            }
        }
    }
}
