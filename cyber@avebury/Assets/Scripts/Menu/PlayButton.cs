using UnityEngine;
using UnityEngine.SceneManagement;

namespace CyberAvebury
{
    public class PlayButton : MonoBehaviour
    {
        public void Play()
        {
            SceneManager.LoadScene(1);
        }
    }
}
