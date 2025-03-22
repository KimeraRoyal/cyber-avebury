using UnityEngine;

namespace CyberAvebury
{
    public class DummyWord : MonoBehaviour
    {
        private DummyPool m_pool;
        
        private RectTransform m_rect;

        public Vector2 Size
        {
            get => m_rect.sizeDelta;
            set => m_rect.sizeDelta = value;
        }

        private void Awake()
        {
            m_pool = GetComponentInParent<DummyPool>();
            
            m_rect = GetComponent<RectTransform>();
        }
        
        public void SwapWith(RectTransform _other)
        {
            if(!_other) { return; }
            
            var aParent = transform.parent;
            var aIndex = transform.GetSiblingIndex();
            var bIndex = _other.GetSiblingIndex();

            transform.parent = _other.parent;
            _other.parent = aParent;
            
            transform.SetSiblingIndex(bIndex);
            _other.SetSiblingIndex(aIndex);
        }
    }
}