using UnityEngine;
using UnityEngine.Events;

namespace CyberAvebury
{
    public class Mouse3D : Mouse
    {
        [SerializeField] private LayerMask m_targetMask;

        public UnityEvent<ClickableObject> OnObjectClicked;

        protected override void Cast(Vector3 _mousePos)
        {
            var ray = Camera.ScreenPointToRay(_mousePos);
            if (!Physics.Raycast(ray, out var rayHit, Camera.farClipPlane, m_targetMask)) { return; }
            
            var clickableObject = rayHit.transform.GetComponentInParent<ClickableObject>();
            if(!clickableObject) { return; }
            
            clickableObject.Click();
            OnObjectClicked?.Invoke(clickableObject);
        }
    }
}
