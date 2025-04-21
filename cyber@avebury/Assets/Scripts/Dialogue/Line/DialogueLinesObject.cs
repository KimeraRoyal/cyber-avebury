using UnityEditor;
using UnityEngine;

namespace CyberAvebury
{
    [CreateAssetMenu(fileName = "Dialogue", menuName = "cyber@avebury/Dialogue/Lines")]
    public class DialogueLinesObject : DialogueLineObjectBase
    {
        [SerializeField] private DialogueLines m_lines;

        public DialogueLines Lines => m_lines;

        public static implicit operator DialogueLines(DialogueLinesObject _linesObject)
            => _linesObject?.Lines;

        public override DialogueLineBase GetLine()
            => m_lines;

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
            m_lines.ResetUses();
        }
#endif
    }
}