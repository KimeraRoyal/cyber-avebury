using UnityEngine;

namespace CyberAvebury
{
    [RequireComponent(typeof(SpriteRenderer))] [ExecuteInEditMode]
    public class RadialFill : MonoBehaviour
    {
        private static readonly int s_angle = Shader.PropertyToID("_Angle");
        private static readonly int s_arcPoint1 = Shader.PropertyToID("_Arc1");
        private static readonly int s_arcPoint2 = Shader.PropertyToID("_Arc2");
        private static readonly int s_pixelSnap = Shader.PropertyToID("PixelSnap");
        
        private SpriteRenderer m_spriteRenderer;

        private MaterialPropertyBlock m_propertyBlock;

        [SerializeField] [Range(0, 360.0f)] private float m_angle = 0.0f;
        [SerializeField] [Range(0, 360.0f)] private float m_arcPoint1 = 0.0f;
        [SerializeField] [Range(0, 360.0f)] private float m_arcPoint2 = 0.0f;
        [SerializeField] private bool m_pixelSnap;
        
        private bool m_dirty;

        private float m_lastAngle;
        private float m_lastArcPoint1;
        private float m_lastArcPoint2;
        private bool m_lastPixelSnap;

        public float Angle
        {
            get => m_angle;
            set
            {
                m_angle = value;
                m_dirty = true;
            }
        }

        public float ArcPoint1
        {
            get => m_arcPoint1;
            set
            {
                m_arcPoint1 = value;
                m_dirty = true;
            }
        }

        public float ArcPoint2
        {
            get => m_arcPoint2;
            set
            {
                m_arcPoint2 = value;
                m_dirty = true;
            }
        }

        public bool PixelSnap
        {
            get => m_pixelSnap;
            set
            {
                m_pixelSnap = value;
                m_dirty = true;
            }
        }
        
        private void Awake()
        {
            m_spriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void Start()
        {
            m_dirty = true;
            UpdatePropertyBlock();
        }

        private void Update()
        {
            if(m_dirty) { return; }
            
            m_dirty |= Mathf.Abs(m_angle - m_lastAngle) > 0.0001f;
            m_dirty |= Mathf.Abs(m_arcPoint1 - m_lastArcPoint1) > 0.0001f;
            m_dirty |= Mathf.Abs(m_arcPoint2 - m_lastArcPoint2) > 0.0001f;
            m_dirty |= m_pixelSnap != m_lastPixelSnap;
        }

        private void LateUpdate()
            => UpdatePropertyBlock();

        private void UpdatePropertyBlock()
        {
            if(!m_dirty) { return; }
            
            m_propertyBlock ??= new MaterialPropertyBlock();
            
            m_propertyBlock.SetFloat(s_angle, m_angle);
            m_propertyBlock.SetFloat(s_arcPoint1, m_arcPoint1);
            m_propertyBlock.SetFloat(s_arcPoint2, m_arcPoint2);
            m_propertyBlock.SetFloat(s_pixelSnap, m_pixelSnap ? 1.0f : 0.0f);
            
            m_lastAngle = m_angle;
            m_lastArcPoint1 = m_arcPoint1;
            m_lastArcPoint2 = m_arcPoint2;
            m_lastPixelSnap = m_pixelSnap;
            
            m_spriteRenderer.SetPropertyBlock(m_propertyBlock);

            m_dirty = false;
        }
    }
}
