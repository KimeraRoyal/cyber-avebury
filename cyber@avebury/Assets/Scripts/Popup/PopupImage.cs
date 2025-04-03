using UnityEngine;
using UnityEngine.UI;

namespace CyberAvebury
{
    [RequireComponent(typeof(Image))]
    public class PopupImage : MonoBehaviour
    {
        private Popup m_popup;

        private Image m_image;

        private void Awake()
        {
            m_popup = GetComponentInParent<Popup>();

            m_image = GetComponent<Image>();
            
            m_popup.OnPopupShown.AddListener(OnPopupShown);
        }

        private void OnPopupShown(PopupInfo _popup)
        {
            m_image.sprite = _popup.Image;
        }
    }
}