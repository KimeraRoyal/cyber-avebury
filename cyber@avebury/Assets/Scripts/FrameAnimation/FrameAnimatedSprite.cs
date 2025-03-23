using UnityEngine;

namespace KR
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class FrameAnimatedSprite : FrameAnimator
    {
        private SpriteRenderer m_renderer;

        private void Awake()
        {
            m_renderer = GetComponent<SpriteRenderer>();
        }

        protected override void SetSprite(Sprite _sprite)
            => m_renderer.sprite = _sprite;
    }
}