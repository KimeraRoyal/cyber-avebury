using CyberAvebury.Minigames;
using UnityEngine;
using UnityEngine.Events;

namespace CyberAvebury
{
    public class MinigameLoader : MonoBehaviour
    {
        private Minigame m_currentMinigame;

        public UnityEvent<Minigame> OnMinigameLoaded;
        public UnityEvent<Minigame> OnMinigameUnloaded;

        public Minigame LoadMinigame(Minigame _minigamePrefab)
        {
            if (m_currentMinigame) { return null; }

            m_currentMinigame = Instantiate(_minigamePrefab, transform);
            m_currentMinigame.OnEnd.AddListener(UnloadMinigame);
            m_currentMinigame.gameObject.SetActive(false);

            LoadingScreen.Instance.ShowScreen(1.0f, () =>
            {
                m_currentMinigame.gameObject.SetActive(true);
                OnMinigameLoaded?.Invoke(m_currentMinigame);
            });
            
            return m_currentMinigame;
        }

        private void UnloadMinigame()
        {
            m_currentMinigame.OnEnd.RemoveListener(UnloadMinigame);

            LoadingScreen.Instance.ShowScreen(1.0f, () =>
            {
                OnMinigameUnloaded?.Invoke(m_currentMinigame);
                Destroy(m_currentMinigame.gameObject);
                m_currentMinigame = null;
            });
        }
    }
}
