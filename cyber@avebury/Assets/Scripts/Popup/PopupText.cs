using System;
using TMPro;
using UnityEngine;

namespace CyberAvebury
{
    [RequireComponent(typeof(TMP_Text))]
    public class PopupText : MonoBehaviour
    {
        private enum Type
        {
            Title,
            Description
        }
        
        private Popup m_popup;

        private TMP_Text m_text;

        [SerializeField] private Type m_type;

        private void Awake()
        {
            m_popup = GetComponentInParent<Popup>();

            m_text = GetComponent<TMP_Text>();
            
            m_popup.OnPopupShown.AddListener(OnPopupShown);
        }

        private void OnPopupShown(PopupInfo _popup)
        {
            m_text.text = m_type switch
            {
                Type.Title => _popup.Title,
                Type.Description => _popup.Description,
                _ => throw new ArgumentOutOfRangeException()
            };
        }
    }
}