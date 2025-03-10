using UnityEngine;

namespace Minigame.Mainframe.Rings
{
    public class Ring : MonoBehaviour
    {
        private float m_maxSize;
        private float m_totalLifetime;

        public float MaxSize
        {
            get => m_maxSize;
            set => m_maxSize = value;
        }

        public float TotalLifetime
        {
            get => m_totalLifetime;
            set => m_totalLifetime = value;
        }
    }
}