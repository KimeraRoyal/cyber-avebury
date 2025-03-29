using UnityEngine;
using UnityEngine.Events;

namespace CyberAvebury
{
    public class ClickableObject : MonoBehaviour
    {
        public UnityEvent OnClicked;

        public void Click()
        {
            OnClicked?.Invoke();
        }
    }
}