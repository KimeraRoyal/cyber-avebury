using CyberAvebury.Minigames;
using UnityEngine;

namespace CyberAvebury
{
    [RequireComponent(typeof(Animator))]
    public class NodeInformation : MonoBehaviour
    {
        private static readonly int s_showVariable = Animator.StringToHash("Show");
        private static readonly int s_minigameVariable = Animator.StringToHash("Minigame");
        private NodeSelection m_selection;
        private MinigameLoader m_loader;

        private Animator m_animator;

        private void Awake()
        {
            m_selection = FindAnyObjectByType<NodeSelection>();
            m_selection.OnNodeSelected.AddListener(OnNodeSelected);
            
            m_loader = FindAnyObjectByType<MinigameLoader>();
            m_loader.OnMinigameLoaded.AddListener(StartMinigame);
            m_loader.OnMinigameUnloaded.AddListener(EndMinigame);

            m_animator = GetComponent<Animator>();
        }

        private void OnNodeSelected(Node _node)
            => m_animator.SetBool(s_showVariable, _node);

        private void StartMinigame(Minigame _minigame)
            => m_animator.SetBool(s_minigameVariable, true);

        private void EndMinigame(Minigame _minigame)
            => m_animator.SetBool(s_minigameVariable, false);
    }
}
