namespace CyberAvebury
{
    public struct DialogueLine
    {
        private DialogueCharacter m_character;
        private string m_line;

        public DialogueCharacter Character => m_character;
        public string Line => m_line;

        public DialogueLine(DialogueCharacter _character, string _line)
        {
            m_character = _character;
            m_line = _line;
        }
    }
}