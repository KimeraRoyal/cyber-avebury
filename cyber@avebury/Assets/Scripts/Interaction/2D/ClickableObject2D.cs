using UnityEngine;

namespace CyberAvebury
{
    // TODO: Write to work off cast rays, similar to in Parabolic Emotion
    [RequireComponent(typeof(Collider2D))]
    public class ClickableObject2D : ClickableObject
    {
        private Mouse2D m_mouse;
        
        private Collider2D m_collider2D;

        private void Awake()
        {
            m_mouse = FindAnyObjectByType<Mouse2D>();
            
            m_collider2D = GetComponent<Collider2D>();
        }

        private void OnEnable()
        {
            m_mouse.OnMouseClickedWorld.AddListener(OnMouseClicked);
        }

        private void OnDestroy()
        {
            m_mouse.OnMouseClickedWorld.RemoveListener(OnMouseClicked);
        }

        private void OnMouseClicked(Vector2 _mousePos)
        {
            if(!m_collider2D.OverlapPoint(_mousePos)) { return; }
            OnClicked?.Invoke();
        }
    }
}
