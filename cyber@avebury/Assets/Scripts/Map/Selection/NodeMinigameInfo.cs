using TMPro;
using UnityEngine;

namespace CyberAvebury
{
    [RequireComponent(typeof(TMP_Text))]
    public class NodeMinigameInfo : MonoBehaviour
    {
        private NodeSelection m_selection;

        private TMP_Text m_text;

        [SerializeField] [TextArea(3, 5)] private string m_layout = "{0}\n{1}";

        private void Awake()
        {
            m_selection = FindAnyObjectByType<NodeSelection>();
            m_selection.OnNodeSelected.AddListener(OnNodeSelected);

            m_text = GetComponent<TMP_Text>();
        }

        private void OnNodeSelected(Node _node)
        {
            if (!_node) { return; }

            var minigame = _node.Minigame;
            m_text.text = string.Format(m_layout, minigame.gameObject.name, minigame.Description);
        }
    }
}