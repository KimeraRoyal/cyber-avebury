using UnityEngine;

namespace CyberAvebury
{
    [RequireComponent(typeof(Animator))]
    public class NodeInformation : MonoBehaviour
    {
        private static readonly int s_showVariable = Animator.StringToHash("Show");
        private NodeSelection m_selection;

        private Animator m_animator;

        private void Awake()
        {
            m_selection = FindAnyObjectByType<NodeSelection>();
            
            m_selection.OnNodeSelected.AddListener(OnNodeSelected);

            m_animator = GetComponent<Animator>();
        }

        private void OnNodeSelected(Node _node)
        {
            m_animator.SetBool(s_showVariable, _node);
        }
    }
}
