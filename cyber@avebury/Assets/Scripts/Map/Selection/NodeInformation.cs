using CyberAvebury.Minigames;
using DG.Tweening;
using UnityEngine;

namespace CyberAvebury
{
    [RequireComponent(typeof(Animator), typeof(CanvasGroup))]
    public class NodeInformation : MonoBehaviour
    {
        private static readonly int s_showVariable = Animator.StringToHash("Show");
        private NodeSelection m_selection;
        private MinigameLoader m_loader;

        private Animator m_animator;
        private CanvasGroup m_canvasGroup;

        private Tween m_fadeTween;

        private void Awake()
        {
            m_selection = FindAnyObjectByType<NodeSelection>();
            m_selection.OnNodeSelected.AddListener(OnNodeSelected);
            
            m_loader = FindAnyObjectByType<MinigameLoader>();
            m_loader.OnMinigameLoaded.AddListener(StartMinigame);
            m_loader.OnMinigameUnloaded.AddListener(EndMinigame);

            m_animator = GetComponent<Animator>();
            m_canvasGroup = GetComponent<CanvasGroup>();
        }

        private void OnNodeSelected(Node _node)
            => m_animator.SetBool(s_showVariable, _node);

        private void StartMinigame(Minigame _minigame)
            => ShowCanvasGroup(false);

        private void EndMinigame(Minigame _minigame)
            => ShowCanvasGroup(true);

        private void ShowCanvasGroup(bool _show)
        {
            if (m_fadeTween is { active: true })
            {
                m_fadeTween.Kill();
            }

            var fade = _show ? 1.0f : 0.0f;
            m_canvasGroup.blocksRaycasts = _show;

            m_fadeTween = DOTween.To(() => m_canvasGroup.alpha, _alpha => m_canvasGroup.alpha = _alpha, fade, 0.2f);
        }
    }
}
