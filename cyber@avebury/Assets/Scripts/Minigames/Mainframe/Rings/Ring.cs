using System;
using UnityEngine;

namespace CyberAvebury.Minigames.Mainframe.Rings
{
    public class Ring : MonoBehaviour
    {
        [SerializeField] private float m_startingSize = 0.1f;
        
        private float m_maxSize;
        
        private float m_totalLifetime;
        private float m_currentLifetime;

        public Action OnPressed;
        public Action OnFailed;

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
            m_currentLifetime += Time.deltaTime;
            transform.localScale = Vector3.one * CurrentSize;
            
            if(m_currentLifetime < m_totalLifetime) { return; }
            
            OnFailed?.Invoke();
            Destroy(gameObject);
        }
    }
}