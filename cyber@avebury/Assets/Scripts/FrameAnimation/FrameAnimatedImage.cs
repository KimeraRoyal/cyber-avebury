using UnityEngine;
using UnityEngine.UI;

namespace KR
{
    [RequireComponent(typeof(Image))]
    public class FrameAnimatedImage : FrameAnimator
    {
        private Image m_image;

        private void Awake()
        {
            m_image = GetComponent<Image>();
        }

        protected override void SetSprite(Sprite _sprite)
            => m_image.sprite = _sprite;
    }
}