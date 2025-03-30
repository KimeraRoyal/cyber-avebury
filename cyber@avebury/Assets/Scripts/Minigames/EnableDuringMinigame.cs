using CyberAvebury.Minigames;
using UnityEngine;

namespace CyberAvebury
{
    public class EnableDuringMinigame : MonoBehaviour
    {
        private MinigameLoader m_loader;

        [SerializeField] private bool m_invert;

        private void Awake()
        {
            m_loader = FindAnyObjectByType<MinigameLoader>();
            m_loader.OnMinigameLoaded.AddListener(OnMinigameLoaded);
            m_loader.OnMinigameUnloaded.AddListener(OnMinigameUnloaded);
        }

        private void OnMinigameLoaded(Minigame _minigame)
        {
            gameObject.SetActive(true ^ m_invert);
        }

        private void OnMinigameUnloaded(Minigame _minigame)
        {
            gameObject.SetActive(false ^ m_invert);
        }
    }
}
