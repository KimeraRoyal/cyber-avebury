using UnityEngine;
using UnityEngine.Events;

namespace CyberAvebury
{
    public class Word : MonoBehaviour
    {
        public UnityEvent OnGrabbed;
        public UnityEvent OnReleased;
        
        public void Grab()
        {
            OnGrabbed?.Invoke();
        }

        public void Release()
        {
            OnReleased?.Invoke();
        }
    }
}
