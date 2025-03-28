using UnityEngine;

namespace CyberAvebury
{
    [RequireComponent(typeof(Animator))]
    public class AnimatorSpeedVariance : MonoBehaviour
    {
        private Animator m_animator;
        
        [SerializeField] private float m_minSpeed = 1.0f;
        [SerializeField] private float m_maxSpeed = 1.0f;

        private void Awake()
        {
            m_animator = GetComponent<Animator>();
        }

        private void Start()
        {
            m_animator.speed = Random.Range(m_minSpeed, m_maxSpeed);
        }
    }
}
