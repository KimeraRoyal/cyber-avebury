using UnityEngine;
using UnityEngine.UI;

namespace CyberAvebury
{
    [RequireComponent(typeof(Button))]
    public class MinigameButton : MonoBehaviour
    {
        private NodeSelection m_selection;
        
        private Button m_button;

        private void Awake()
        {
            m_selection = FindAnyObjectByType<NodeSelection>();
            m_selection.OnNodeSelected.AddListener(OnNodeSelected);

            m_button = GetComponent<Button>();
            m_button.onClick.AddListener(ClickButton);
        }

        private void OnNodeSelected(Node _node)
        {
            m_button.interactable = _node;
        }

        private void ClickButton()
        {
            m_selection.SelectedNode.Enter();
            m_selection.LoadSelectedMinigame();
        }
    }
}
