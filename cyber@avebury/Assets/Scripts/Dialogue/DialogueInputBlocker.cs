using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace CyberAvebury
{
    [RequireComponent(typeof(Image))]
    public class DialogueInputBlocker : MonoBehaviour
    {
        private Dialogue m_dialogue;

        private Image m_image;

        [SerializeField] private float m_visibleAlpha = 0.1f;
        [SerializeField] private float m_fadeDuration = 0.5f;

        private Tween m_fadeTween;

        private void Awake()
        {
            m_dialogue = GetComponentInParent<Dialogue>();
            m_dialogue.OnNewDialogue.AddListener(OnNewDialogue);
            m_dialogue.OnEndDialogue.AddListener(OnEndDialogue);

            m_image = GetComponent<Image>();
        }

        private void OnNewDialogue(DialogueLineBase _line)
        {
            m_image.raycastTarget = true;
            Fade(m_visibleAlpha);
        }

        private void OnEndDialogue()
        {
            m_image.raycastTarget = false;
            Fade(0.0f);
        }

        private void Fade(float _amount)
        {
            if (m_fadeTween is { active: true }) { m_fadeTween.Kill(); }
            m_fadeTween = m_image.DOFade(_amount, m_fadeDuration);
        }
    }
}
