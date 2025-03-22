using UnityEngine;
using UnityEngine.Events;

namespace CyberAvebury
{
    public class WordGrabber : MonoBehaviour
    {
        private MouseRaycaster m_mouseRaycaster;

        private DummyPool m_dummies;
        private CopyMousePosition m_mousePosition;

        [SerializeField] private RectTransform m_passwordLine;
        
        private Word m_grabbedWord;
        private WordLine m_hoveredLine;
        
        private bool m_wasMouseButtonHeld;
        private Vector2 m_lastMousePosition;
        
        public Word GrabbedWord => m_grabbedWord;

        public UnityEvent<Word> OnWordGrabbed;
        public UnityEvent<Word> OnWordReleased;

        private void Awake()
        {
            m_mouseRaycaster = GetComponentInParent<MouseRaycaster>();
            
            m_dummies = GetComponentInChildren<DummyPool>();
            m_mousePosition = GetComponentInChildren<CopyMousePosition>();
        }

        private void Update()
        {
            Grab();
            Release();
            Drag();
            
            m_wasMouseButtonHeld = Input.GetMouseButton(0);
            m_lastMousePosition = Input.mousePosition;
        }
 
        private void Grab()
        {
            var mouseDown = Input.GetMouseButton(0) && !m_wasMouseButtonHeld;
            if(!mouseDown) { return; }
            
            m_grabbedWord = m_mouseRaycaster.GetFirstRaycastComponent<Word>();
            if (!m_grabbedWord) { return; }
            
            CreatePasswordDummy();

            if (m_grabbedWord.PasswordDummy)
            {
                m_grabbedWord.PasswordDummy.SwapWith(m_grabbedWord.RectTransform);
            }
            else
            {
                var dummy = m_dummies.Get();
                dummy.Size = m_grabbedWord.RectTransform.sizeDelta;
                m_grabbedWord.Dummy = dummy;
                dummy.SwapWith(m_grabbedWord.RectTransform);
            }
            
            m_grabbedWord.Grab();
            OnWordGrabbed?.Invoke(m_grabbedWord);
        }

        private void Release()
        {
            var mouseUp = !Input.GetMouseButton(0) && m_wasMouseButtonHeld;
            if(!mouseUp || !m_grabbedWord) { return; }

            if (m_grabbedWord.PasswordDummy)
            {
                var dummy = m_grabbedWord.PasswordDummy;
                dummy.SwapWith(m_grabbedWord.RectTransform);
            }
            else
            {
                var dummy = m_grabbedWord.Dummy;
                m_grabbedWord.Dummy = null;
                dummy.SwapWith(m_grabbedWord.RectTransform);
                m_dummies.Release(dummy);
            }
                
            ReleasePasswordDummy();

            var releasedWord = m_grabbedWord;
            m_grabbedWord = null;
            
            releasedWord.Release();
            OnWordReleased?.Invoke(releasedWord);
        }

        private void Drag()
        {
            var didMouseMove = ((Vector2)Input.mousePosition - m_lastMousePosition).magnitude > 0.0001f;
            if(!m_grabbedWord || !didMouseMove) { return; }

            if (m_grabbedWord.RectTransform.position.y <= m_passwordLine.position.y)
            {
                ReleasePasswordDummy();
            }
            
            m_hoveredLine = m_mouseRaycaster.GetFirstRaycastComponent<WordLine>();
            if (!m_hoveredLine) { return; }

            CreatePasswordDummy();
            if(!m_grabbedWord.PasswordDummy) { return; }
            m_grabbedWord.PasswordDummy.transform.parent = m_hoveredLine.transform;
        }

        private void CreatePasswordDummy()
        {
            if(m_grabbedWord.PasswordDummy || m_grabbedWord.RectTransform.position.y < m_passwordLine.position.y) { return; }

            var dummy = m_dummies.Get();
            dummy.Size = m_grabbedWord.RectTransform.sizeDelta;
            m_grabbedWord.PasswordDummy = dummy;
        }

        private void ReleasePasswordDummy()
        {
            if(!m_grabbedWord.PasswordDummy) { return; }
            
            m_dummies.Release(m_grabbedWord.PasswordDummy);
            m_grabbedWord.PasswordDummy = null;
        }
    }
}
