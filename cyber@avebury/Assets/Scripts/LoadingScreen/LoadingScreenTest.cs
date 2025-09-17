using UnityEngine;

namespace CyberAvebury.LoadingScreen
{
    public class LoadingScreenTest : MonoBehaviour
    {
        private bool m_glitch;
        
        public void ToggleShow()
        {
            LoadingScreen.Instance.ShowScreen(3.0f);
        }

        public void ToggleGlitch()
        {
            m_glitch = !m_glitch;
            LoadingScreen.Instance.GlitchScreen(m_glitch);
        }
    }
}
