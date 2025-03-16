using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CyberAvebury
{
    public class WordGrabber : MonoBehaviour
    {
        private GraphicRaycaster m_raycaster;

        private CopyMousePosition m_mousePosition;

        [SerializeField] private RectTransform m_dummy;
        
        private Word m_grabbedWord;

        public Word GrabbedWord => m_grabbedWord;

        public UnityEvent<Word> OnWordGrabbed;
        public UnityEvent<Word> OnWordReleased;

        private void Awake()
        {
            m_raycaster = GetComponentInParent<GraphicRaycaster>();

            m_mousePosition = GetComponentInChildren<CopyMousePosition>();
        }

        private void Update()
        {
            Grab();
            Release();
        }

        private void Grab()
        {
            if(!Input.GetMouseButtonDown(0)) { return; }
            
            m_grabbedWord = Raycast();
            if (!m_grabbedWord) { return; }

            var grabbedTransform = m_grabbedWord.transform as RectTransform;
            if(!grabbedTransform) { return; }
            
            m_dummy.sizeDelta = grabbedTransform.sizeDelta;
            Swap(m_grabbedWord.transform as RectTransform, m_dummy);
            
            m_grabbedWord.Grab();
            OnWordGrabbed?.Invoke(m_grabbedWord);
        }

        private void Release()
        {
            if(!(Input.GetMouseButtonUp(0) && m_grabbedWord)) { return; }
            
            Swap(m_grabbedWord.transform as RectTransform, m_dummy);

            var releasedWord = m_grabbedWord;
            m_grabbedWord = null;
            
            releasedWord.Release();
            OnWordReleased?.Invoke(releasedWord);
        }

        private void Swap(RectTransform _a, RectTransform _b)
        {
            var aParent = _a.parent;
            var aIndex = _a.GetSiblingIndex();
            var bIndex = _b.GetSiblingIndex();

            _a.parent = _b.parent;
            _b.parent = aParent;
            
            _a.SetSiblingIndex(bIndex);
            _b.SetSiblingIndex(aIndex);
        }

        private Word Raycast()
        {
            var eventData = new PointerEventData(null)
            {
                position = Input.mousePosition
            };

            var results = new List<RaycastResult>();
            m_raycaster.Raycast(eventData, results);

            foreach (var result in results)
            {
                var word = result.gameObject.GetComponent<Word>();
                if (word) { return word; }
            }
            return null;
        }
    }
}
