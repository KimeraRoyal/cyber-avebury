using UnityEngine;

namespace CyberAvebury
{
    public class PopupOnStart : MonoBehaviour
    {
        private Popup m_popup;
        
        [SerializeField] private PopupInfo m_info;

        private void Awake()
        {
            m_popup = FindAnyObjectByType<Popup>();
        }

        private void Start()
        {
            m_popup.Show(m_info);
        }
    }
}