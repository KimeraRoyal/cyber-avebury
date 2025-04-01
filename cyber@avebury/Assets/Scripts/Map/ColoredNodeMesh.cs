using UnityEngine;

namespace CyberAvebury
{
    [RequireComponent(typeof(MeshRenderer))]
    public class ColoredNodeMesh : MonoBehaviour
    {
        private NodeColorizer m_colorizer;
        
        private MeshRenderer m_renderer;

        [SerializeField] private int m_materialIndex = 0;
        [SerializeField] private string m_propertyName = "_Color";
        private MaterialPropertyBlock m_block;
        private int m_propertyHash;

        private void Awake()
        {
            m_colorizer = GetComponentInParent<NodeColorizer>();

            m_renderer = GetComponent<MeshRenderer>();
            m_block = new MaterialPropertyBlock();
            m_propertyHash = Shader.PropertyToID(m_propertyName);
            
            m_colorizer.OnColorUpdated.AddListener(UpdateColor);
        }

        private void Start()
        {
            UpdateColor(m_colorizer.CurrentColor);
        }

        private void UpdateColor(Color _color)
        {
            m_renderer.GetPropertyBlock(m_block);
            m_block.SetColor(m_propertyHash, _color);
            m_renderer.SetPropertyBlock(m_block);
        }
    }
}
