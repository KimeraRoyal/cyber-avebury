using UnityEngine;
using UnityEngine.EventSystems;

namespace CyberAvebury
{
    public class File : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        private MouseRaycaster m_raycaster;
        
        private FileRegion m_currentRegion;

        private RectTransform m_rect;
        
        [SerializeField] private GameObject m_graphic;
        [SerializeField] private GameObject m_marchingAnts;
        
        [SerializeField] private GameObject m_dummy;

        private bool m_grabbable = true;
        private bool m_selected;
        private bool m_grabbed;
        
        public bool Grabbable
        {
            get => m_grabbable;
            set => m_grabbable = value;
        }

        private void Awake()
        {
            m_raycaster = GetComponentInParent<MouseRaycaster>();
            
            m_currentRegion = GetComponentInParent<FileRegion>();
            
            m_rect = GetComponent<RectTransform>();
        }

        private void Start()
        { 
            m_marchingAnts.SetActive(false);
            m_dummy.SetActive(false);
        }

        private void Update()
        {
            if (m_grabbable && !m_grabbed && m_selected && Input.GetMouseButtonDown(0))
            {
                m_dummy.transform.position = transform.position;
                
                m_dummy.SetActive(true);
                m_graphic.SetActive(false);
                m_grabbed = true;
            }

            if (m_grabbed && Input.GetMouseButtonUp(0))
            {
                var newRegion = m_raycaster.GetFirstRaycastComponent<FileRegion>();
                if (newRegion)
                {
                    m_currentRegion = newRegion;
                    transform.parent = m_currentRegion.transform;
                    m_currentRegion.AddFile(this);
                }
                
                transform.position = RoundVector(m_dummy.transform.position);
                m_currentRegion.ClampRect(m_rect);
                
                m_dummy.SetActive(false);
                m_graphic.SetActive(true);
                m_grabbed = false;
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            m_marchingAnts.SetActive(true);
            m_selected = true;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if(m_grabbed) { return; }
            m_marchingAnts.SetActive(false);
            m_selected = false;
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
