using UnityEngine;
using UnityEngine.Events;

namespace CyberAvebury
{
    public class Mouse2D : Mouse
    {
        public UnityEvent<Vector2> OnMouseClickedWorld;
        
        protected override void Click(Vector3 _mousePos)
        {
            var worldPos = Camera.ScreenToWorldPoint(_mousePos);
            OnMouseClickedWorld?.Invoke(worldPos);
        }
    }
}