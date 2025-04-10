using UnityEditor;
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

#if UNITY_EDITOR
        private void OnEnable()
        {
            EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
        }

        private void OnDisable()
        {
            EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
        }

        private void OnPlayModeStateChanged(PlayModeStateChange change)
        {
            foreach (var lines in m_lines)
            {
                lines.ResetUses();
            }
        }
#endif
    }
}
