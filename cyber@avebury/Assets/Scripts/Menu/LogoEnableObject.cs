using UnityEngine;

namespace CyberAvebury
{
    public class LogoEnableObject : MonoBehaviour
    {
        private static readonly int s_appearVariable = Animator.StringToHash("Appear");
        
        [SerializeField] private Animator[] m_animators;
        
        public void EnableObject(int _index)
        {
            m_animators[_index].SetTrigger(s_appearVariable);
        }
    }
}
