using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

namespace CyberAvebury
{
    public class InverseMask : Image
    {
        private static readonly int s_stencilComp = Shader.PropertyToID("_StencilComp");

        private Material m_material;

        [OnValueChanged("SetCompareFunction")]
        [SerializeField] private bool m_invert;
        private bool m_wasInverted;

        public override Material materialForRendering
        {
            get
            {
                m_material = new Material(base.materialForRendering);
                SetCompareFunction();
                return m_material;
            }
        }

        private void Update()
        {
            if(m_invert == m_wasInverted) { return; }
            SetCompareFunction();
            m_wasInverted = m_invert;
        }

        private void SetCompareFunction()
        {
            m_material.SetInt(s_stencilComp, (int) (m_invert ? CompareFunction.NotEqual : CompareFunction.Equal));
        }
    }
}
