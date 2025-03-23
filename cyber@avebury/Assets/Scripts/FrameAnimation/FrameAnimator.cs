using UnityEngine;

namespace KR
{
    public abstract class FrameAnimator : MonoBehaviour
    {
        [SerializeField] private FrameAnimation m_animation;
        [SerializeField] private int m_frameOffset;

        private float m_frameDuration;

        private int m_currentFrame;
        private float m_timer;

        public FrameAnimation Animation
        {
            get => m_animation;
            set
            {
                if (m_animation == value) { return; }

                m_animation = value;
                if (m_animation) { m_frameDuration = 1.0f / m_animation.FPS; }

                ResetAnimation();
            }
        }

        public void ResetAnimation()
        {
            if(!m_animation) { return; }
            
            m_timer = 0.0f;
            m_currentFrame = 0;
            ChangeFrame(0);
        }

        private void Start()
        {
            ResetAnimation();
        }

        private void Update()
        {
            if(!m_animation) { return; }
            
            m_timer += Time.deltaTime;

            var updateFrame = false;
            while (m_timer >= m_frameDuration)
            {
                m_timer -= m_frameDuration;
                updateFrame = true;
            }
            if(!updateFrame) { return; }

            ChangeFrame(m_currentFrame++);
        }

        private void ChangeFrame(int _currentFrame)
        {
            _currentFrame += m_frameOffset;
            SetSprite(m_animation.GetFrame(_currentFrame));
        }

        protected abstract void SetSprite(Sprite _sprite);
    }
}
