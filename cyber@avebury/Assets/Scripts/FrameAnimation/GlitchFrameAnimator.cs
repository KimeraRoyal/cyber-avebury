using KR;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

namespace CyberAvebury
{
    public class GlitchFrameAnimator : MonoBehaviour
    {
        private FrameAnimator m_animator;

        [SerializeField] private float m_minSpeed = 1.0f, m_maxSpeed = 1.0f;
        [SerializeField] private float m_minRandomizeSpeedInterval = 1.0f, m_maxRandomizeSpeedInterval = 1.0f;
        private float m_randomizeSpeedInterval;
        private float m_randomizeSpeedTimer;

        [SerializeField] private float m_minRandomizeFrameInterval = 1.0f, m_maxRandomizeFrameInterval = 1.0f;
        private float m_randomizeFrameInterval;
        private float m_randomizeFrameTimer;

        [SerializeField] private Vector2 m_minOffset, m_maxOffset;
        [SerializeField] private float m_minRandomizeOffsetInterval = 1.0f, m_maxRandomizeOffsetInterval = 1.0f;
        private float m_randomizeOffsetInterval;
        private float m_randomizeOffsetTimer;

        [OnValueChanged("UpdateGlitching")]
        [SerializeField] private bool m_glitching;
        private bool m_wasGlitching;

        private Vector2 m_defaultOffset;
        private float m_defaultSpeed;
        private int m_defaultFrameOffset;

        private void Awake()
        {
            m_animator = GetComponent<FrameAnimator>();
        }

        private void Start()
        {
            m_defaultOffset = transform.localPosition;
            m_defaultSpeed = m_animator.Speed;
            m_defaultFrameOffset = m_animator.FrameOffset;

            if (m_glitching) { UpdateGlitching(); }
        }

        private void Update()
        {
            UpdateGlitching();
            
            if(!m_glitching) { return; }
            
            m_randomizeSpeedTimer += Time.deltaTime;
            m_randomizeFrameTimer += Time.deltaTime;
            m_randomizeOffsetTimer += Time.deltaTime;
            
            RandomizeSpeed();
            RandomizeFrameOffset();
            RandomizeOffset();
        }

        public void UpdateGlitching()
        {
            if(!m_animator || m_glitching == m_wasGlitching) { return; }
            
            if(m_glitching) { StartGlitching(); }
            else { StopGlitching(); }

            m_wasGlitching = m_glitching;
        }

        public void StartGlitching()
        {
            m_randomizeSpeedInterval = Random.Range(m_minRandomizeSpeedInterval, m_maxRandomizeSpeedInterval);
            m_randomizeFrameInterval = Random.Range(m_minRandomizeFrameInterval, m_maxRandomizeFrameInterval);
            m_randomizeOffsetInterval = Random.Range(m_minRandomizeOffsetInterval, m_maxRandomizeOffsetInterval);
            
            m_randomizeSpeedTimer = Random.Range(0.0f, m_randomizeSpeedInterval * 1.25f);
            m_randomizeFrameTimer = Random.Range(0.0f, m_randomizeFrameInterval * 1.25f);
            m_randomizeOffsetTimer = Random.Range(0.0f, m_randomizeOffsetInterval * 1.25f);
            
            RandomizeSpeed();
            RandomizeFrameOffset();
            RandomizeOffset();
        }

        public void StopGlitching()
        {
            m_animator.Speed = m_defaultSpeed;
            m_animator.FrameOffset = m_defaultFrameOffset;
            m_animator.ResetAnimation();

            transform.localPosition = m_defaultOffset;
        }

        private void RandomizeSpeed()
        {
            if(m_randomizeSpeedTimer < m_randomizeSpeedInterval) { return; }
            m_randomizeSpeedTimer -= m_randomizeSpeedInterval;
            m_randomizeSpeedInterval = Random.Range(m_minRandomizeSpeedInterval, m_maxRandomizeSpeedInterval);

            m_animator.Speed = Random.Range(m_minSpeed, m_maxSpeed);
        }

        private void RandomizeFrameOffset()
        {
            if(m_randomizeFrameTimer < m_randomizeFrameInterval) { return; }
            m_randomizeFrameTimer -= m_randomizeFrameInterval;
            m_randomizeFrameInterval = Random.Range(m_minRandomizeFrameInterval, m_maxRandomizeFrameInterval);

            m_animator.FrameOffset = Random.Range(0, m_animator.Animation.Frames.Length);
        }

        private void RandomizeOffset()
        {
            if(m_randomizeOffsetTimer < m_randomizeOffsetInterval) { return; }
            m_randomizeOffsetTimer -= m_randomizeOffsetInterval;
            m_randomizeOffsetInterval = Random.Range(m_minRandomizeOffsetInterval, m_maxRandomizeOffsetInterval);

            var offset = Vector2.zero;
            for (var i = 0; i < 2; i++)
            {
                offset[i] = Random.Range(m_minOffset[i], m_maxOffset[i]);
            }
            transform.localPosition = offset;
        }
    }
}
