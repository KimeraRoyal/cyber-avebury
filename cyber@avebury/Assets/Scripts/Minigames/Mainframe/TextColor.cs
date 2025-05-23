using CyberAvebury.Minigames.Mainframe;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace CyberAvebury
{
    [RequireComponent(typeof(TMP_Text))]
    public class TextColor : MonoBehaviour
    {
        private Mainframe m_mainframe;
        
        private TMP_Text m_text;

        [SerializeField] private Gradient m_gradient;
        [SerializeField] private float m_tweenDuration = 0.1f;

        private Tween m_backgroundColorTween;
        private float m_gradientProgress;

        private float GradientProgress
        {
            get => m_gradientProgress;
            set
            {
                m_gradientProgress = value;
                m_text.color = m_gradient.Evaluate(value);
            }
        }

        private void Awake()
        {
            m_mainframe = GetComponentInParent<Mainframe>();
            
            m_text = GetComponent<TMP_Text>();
            
            m_mainframe.OnScoreUpdated.AddListener(OnScoreUpdated);
        }

        private void Start()
            => m_text.color = m_gradient.Evaluate(0);

        private void OnScoreUpdated(int _score)
            => UpdateColor(m_mainframe.ScoreProgress);

        private void UpdateColor(float _t)
        {
            if (m_backgroundColorTween is { active: true })
            {
                m_backgroundColorTween.Kill();
            }

            m_backgroundColorTween = DOTween.To
            (
                () => GradientProgress,
                _value => GradientProgress = _value,
                _t, m_tweenDuration
            );
        }
    }
}
