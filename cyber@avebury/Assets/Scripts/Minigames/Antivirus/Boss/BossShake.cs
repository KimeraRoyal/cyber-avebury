using UnityEngine;

namespace CyberAvebury
{
    public class BossShake : MonoBehaviour
    {
        [SerializeField] private Vector3 m_shakeDistance;
        [SerializeField] private float m_shakeInterval = 0.1f;
        private float m_shakeTimer;
        
        [SerializeField] private bool m_shake;
        private bool m_wasShaking;

        private void Update()
        {
            if (m_shake && !m_wasShaking)
            {
                m_wasShaking = true;

                m_shakeTimer = 0.0f;
                Shake();
            }
            ShakeTimer();
        }

        private void ShakeTimer()
        {
            if(!m_shake) { return; }

            m_shakeTimer += Time.deltaTime;
            if(m_shakeTimer < m_shakeInterval) { return; }
            m_shakeTimer -= m_shakeInterval;
            
            Shake();
        }

        private void Shake()
        {
            var position = Vector3.zero;
            for (var i = 0; i < 3; i++)
            {
                position[i] = Random.Range(-m_shakeDistance[i], m_shakeDistance[i]);
            }
            transform.localPosition = position;
        }
    }
}
