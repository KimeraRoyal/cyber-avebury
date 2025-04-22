using UnityEngine;
using UnityEngine.SceneManagement;

namespace CyberAvebury
{
    public class PlayButton : MonoBehaviour
    {
        public void Play()
        {
            LoadingScreen.Instance.ShowScreen(1.0f, () => SceneManager.LoadScene(1));
        }
    }
}
