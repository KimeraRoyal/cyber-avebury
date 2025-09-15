using CyberAvebury.Minigames;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CyberAvebury
{
    [RequireComponent(typeof(Minigame))]
    public class TestMinigame : MonoBehaviour
    {
        private Minigame m_minigame;

        [SerializeField] private bool m_testOnStart;
        [SerializeField] [Range(0.0f, 1.0f)] private float m_difficulty;

        private void Awake()
        {
            m_minigame = GetComponent<Minigame>();
        }

        private void Start()
        {
            if (Debug.isDebugBuild && m_testOnStart) { BeginGame(); }
        }

        [Button(name: "Begin Game")]
        public void BeginGame()
        {
            if(m_minigame.IsPlaying) { return; }
            m_minigame.Begin(m_difficulty);
        }
    }
}
