using UnityEngine;
using UnityEngine.Events;

namespace CyberAvebury
{
    public class File : MonoBehaviour
    {
        private InputHandling m_input;
        
        private Canvas m_canvas;
        private MouseRaycaster m_raycaster;
        
        private FileRegion m_currentRegion;

        private RectTransform m_rect;

        [SerializeField] private RectTransform m_graphic;
        [SerializeField] private GameObject m_marchingAnts;

        [SerializeField] private RectTransform m_mouse;
        [SerializeField] private RectTransform m_dummy;

        [SerializeField] private bool m_grabbable = true;
        private bool m_grabbed;
        
        public bool Grabbable
        {
            get => m_grabbable;
            set => m_grabbable = value;
        }

        public UnityEvent OnGrabbed;
        public UnityEvent OnReleased;

        private void Awake()
        {
            m_input = FindAnyObjectByType<InputHandling>();
            m_input.OnPress.AddListener(OnMousePressed);
            m_input.OnUnpress.AddListener(OnMouseReleased);
            
            m_canvas = GetComponentInParent<Canvas>();
            m_raycaster = GetComponentInParent<MouseRaycaster>();
            
            m_currentRegion = GetComponentInParent<FileRegion>();
            
            m_rect = GetComponent<RectTransform>();
        }
        
        private void Start()
        { 
            m_marchingAnts.SetActive(false);
            
            m_dummy.transform.SetParent(m_rect);
            m_dummy.anchoredPosition = Vector2.zero;
            m_dummy.gameObject.SetActive(false);
        }

        private void OnMousePressed(Vector2 _position)
        {
            if (!m_grabbable || m_grabbed) { return; }
            
            RectTransformUtility.ScreenPointToLocalPointInRectangle(m_graphic, _position, null, out var localPoint);
            var contains =
                localPoint.x >= m_graphic.rect.xMin && localPoint.x <= m_graphic.rect.xMax &&
                localPoint.y >= m_graphic.rect.yMin && localPoint.y <= m_graphic.rect.yMax;
            if(!contains) { return; }
            
            m_marchingAnts.SetActive(true);
            
            m_mouse.anchoredPosition = m_input.PointerPosition / m_canvas.scaleFactor;
            m_dummy.transform.SetParent(m_mouse);
            m_dummy.gameObject.SetActive(true);
            
            m_graphic.gameObject.SetActive(false);
            m_grabbed = true;
            OnGrabbed?.Invoke();
        }

        private void OnMouseReleased(Vector2 _position)
        {
            if (!m_grabbed) { return; }
            m_marchingAnts.SetActive(false);
            
            var newRegion = m_raycaster.GetFirstRaycastComponent<FileRegion>();
            if (newRegion)
            {
                m_currentRegion = newRegion;
                transform.SetParent(m_currentRegion.transform);
                m_currentRegion.AddFile(this);
            }
                
            transform.position = RoundVector(m_dummy.transform.position);
            m_currentRegion.ClampRect(m_rect);
                
            m_dummy.transform.SetParent(m_rect);
            m_dummy.anchoredPosition = Vector2.zero;
            m_dummy.gameObject.SetActive(false);
            
            m_graphic.gameObject.SetActive(true);
            m_grabbed = false;
            OnReleased?.Invoke();
        }

        private Vector2 RoundVector(Vector2 _value)
        {
            for (var i = 0; i < 2; i++)
            {
                _value[i] = Mathf.Round(_value[i]);
            }
            return _value;
        }
    }
}
