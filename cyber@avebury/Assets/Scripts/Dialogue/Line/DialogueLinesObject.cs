using UnityEngine;

namespace CyberAvebury
{
    [CreateAssetMenu(fileName = "Dialogue", menuName = "cyber@avebury/Dialogue")]
    public class DialogueLinesObject : ScriptableObject
    {
        [SerializeField] private DialogueLines m_lines;

        public DialogueLines Lines => m_lines;
        
        public static implicit operator DialogueLines(DialogueLinesObject _linesObject)
        {
            return _linesObject.Lines;
        }
    }
}