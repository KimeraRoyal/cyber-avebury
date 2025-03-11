using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace CyberAvebury.Minigames.Mainframe.Rings
{
    public class Ring : MonoBehaviour
    {
        [SerializeField] private float m_startingSize = 0.1f;
        
        private float m_maxSize;
        
        private float m_totalLifetime;
        private float m_currentLifetime;

        private bool m_active;
        
        public UnityEvent<float> OnPressed;
        public UnityEvent OnFailed;
        
        public UnityEvent OnDeactivated;
        
        public float StartingSize
        {
            get => m_startingSize;
            set => m_startingSize = value;
        }
        
        public float MaxSize
        {
            get => m_maxSize;
            set => m_maxSize = value;
        }

        public float CurrentSize => Mathf.Lerp(m_startingSize, m_maxSize, Progress);

        public float TotalLifetime
        {
            get => m_totalLifetime;
            set => m_totalLifetime = value;
        }

        public float Progress => m_currentLifetime / m_totalLifetime;

        private void Start()
        {
            transform.localScale = Vector3.one * m_startingSize;
        }

        private void Update()
        {
            if(!m_active) { return; }
            
            m_currentLifetime += Time.deltaTime;
            transform.localScale = Vector3.one * CurrentSize;
            
            if(m_currentLifetime < m_totalLifetime) { return; }
            
            OnFailed?.Invoke();
            m_active = false;
            
            Deactivate();
        }

        public void Press()
        {
            if(!m_active) { return; }
            
            OnPressed?.Invoke(Progress);
            m_active = false;
            
            Deactivate();
        }

        public void Activate()
        {
            m_active = true;
            m_currentLifetime = 0;
        }

        private void Deactivate()
        {
            OnDeactivated?.Invoke();
        }
    }
}