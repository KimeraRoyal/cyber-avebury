using UnityEngine;
using UnityEngine.UI;

namespace CyberAvebury
{
    [RequireComponent(typeof(Image), typeof(AspectRatioFitter))] [ExecuteInEditMode]
    public class ImageAspectRatio : MonoBehaviour
    {
        private Image m_image;
        private AspectRatioFitter m_fitter;

        private Sprite m_previousSprite;

        private void Awake()
        {
            m_image = GetComponent<Image>();
            m_fitter = GetComponent<AspectRatioFitter>();
        }

        private void Update()
        {
            if(!m_image || m_previousSprite == m_image.sprite) { return; }

            var aspectRatio = 1.0f;
            if (m_image.sprite)
            {
                aspectRatio = m_image.sprite.rect.width / m_image.sprite.rect.height;
            }
            m_fitter.aspectRatio = aspectRatio;
            m_previousSprite = m_image.sprite;
        }
    }
}
