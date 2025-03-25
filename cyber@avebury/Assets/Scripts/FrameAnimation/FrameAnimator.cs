using UnityEngine;

namespace KR
{
    public abstract class FrameAnimator : MonoBehaviour
    {
        [SerializeField] private FrameAnimation m_animation;

        [SerializeField] private float m_speed = 1.0f;
        [SerializeField] private int m_frameOffset;

        private float m_frameDuration;

        private int m_currentFrame;
        private float m_timer;

        public FrameAnimation Animation
        {
            get => m_animation;
            set
            {
                m_animation = value;
                if (m_animation) { m_frameDuration = 1.0f / m_animation.FPS; }

                ResetAnimation();
            }
        }

        public float Speed
        {
            get => m_speed;
            set => m_speed = value;
        }

        public int FrameOffset
        {
            get => m_frameOffset;
            set
            {
                if(m_frameOffset == value) { return; }
                m_frameOffset = value;

                ChangeFrame(m_currentFrame);
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
            Animation = m_animation;
        }

        private void Update()
        {
            if(!m_animation) { return; }

            m_timer += Time.deltaTime * m_speed;
            
            var updateFrame = false;
            for (var i = 0; i < 10 && m_timer >= m_frameDuration; i++)
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
