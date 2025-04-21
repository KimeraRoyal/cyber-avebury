using UnityEngine;
using UnityEngine.SceneManagement;

namespace CyberAvebury
{
    public class PlayButton : MonoBehaviour
    {
        private LoadingScreen m_loadingScreen;

        private void Awake()
        {
            m_loadingScreen = FindAnyObjectByType<LoadingScreen>();
        }

        public void Play()
        {
            m_loadingScreen.ShowScreen(1.0f, () => SceneManager.LoadScene(1));
        }
    }
}
