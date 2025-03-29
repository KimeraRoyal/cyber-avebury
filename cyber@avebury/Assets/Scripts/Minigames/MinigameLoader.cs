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
            m_currentMinigame.OnFinished.AddListener(UnloadMinigame);
            
            OnMinigameLoaded?.Invoke(m_currentMinigame);
            
            return m_currentMinigame;
        }

        private void UnloadMinigame()
        {
            m_currentMinigame.OnFinished.RemoveListener(UnloadMinigame);
            
            OnMinigameUnloaded?.Invoke(m_currentMinigame);
            
            Destroy(m_currentMinigame);
        }
    }
}
