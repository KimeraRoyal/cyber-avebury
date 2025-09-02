using UnityEngine;
using UnityEngine.Events;

namespace CyberAvebury
{
    public class MiniObelisk : MonoBehaviour
    {
        private Obelisk m_obelisk;

        public UnityEvent<NodeState> OnObeliskStateChanged;
        
        private void Start()
        {
            m_obelisk = FindAnyObjectByType<Obelisk>();
            m_obelisk.OnStateChanged.AddListener(OnStateChanged);
        }

        private void OnStateChanged(NodeState _state)
        {
            OnObeliskStateChanged?.Invoke(_state);
        }
    }
}
