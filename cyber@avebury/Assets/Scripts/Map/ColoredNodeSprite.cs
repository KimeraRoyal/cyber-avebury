using System;
using UnityEngine;

namespace CyberAvebury
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class ColoredNodeSprite : MonoBehaviour
    {
        private NodeColorizer m_colorizer;
        
        private SpriteRenderer m_spriteRenderer;

        private void Awake()
        {
            m_colorizer = GetComponentInParent<NodeColorizer>();

            m_spriteRenderer = GetComponent<SpriteRenderer>();
            
            m_colorizer.OnColorUpdated.AddListener(UpdateColor);
        }

        private void Start()
        {
            UpdateColor(m_colorizer.CurrentColor);
        }

        private void UpdateColor(Color _color)
        {
            m_spriteRenderer.color = _color;
        }
    }
}
