using UnityEngine;

namespace CyberAvebury
{
    [CreateAssetMenu(fileName = "Random Dialogue Group", menuName = "cyber@avebury/Dialogue/Random")]
    public class RandomDialogueGroup : DialogueLineObjectBase
    {
        [SerializeField] private DialogueLines[] m_lines;

        public DialogueLines GetRandomLines()
            => m_lines[Random.Range(0, m_lines.Length)];

        public override DialogueLineBase GetLine()
            => GetRandomLines();
    }
}
