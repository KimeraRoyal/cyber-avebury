using CyberAvebury.Minigames;
using DG.Tweening;
using UnityEngine;

namespace CyberAvebury
{
    [RequireComponent(typeof(CanvasGroup))]
    public class TimerFade : MonoBehaviour
    {
        private Minigame m_minigame;
        
        private CanvasGroup m_group;

        [SerializeField] private float m_fadeTime = 1.0f;

        private void Awake()
        {
            m_minigame = GetComponentInParent<Minigame>();
            
            m_group = GetComponent<CanvasGroup>();
            
            m_minigame.OnFinished.AddListener(OnFinished);
        }

        private void OnFinished()
        {
            m_group.DOFade(0.0f, m_fadeTime);
        }
    }
}
