using CyberAvebury.Minigames.Mainframe;
using UnityEngine;

namespace CyberAvebury
{
    public class ConsoleTyper : MonoBehaviour
    {
        private Mainframe m_mainframe;

        private Console m_console;

        private int m_lastScore;

        [SerializeField] private string[] m_commands;

        [SerializeField] private int m_minLines = 1;
        [SerializeField] private int m_maxLines = 3;
        [SerializeField] private float m_linesMult = 0.75f;

        private void Awake()
        {
            m_mainframe = GetComponentInParent<Mainframe>();

            m_console = GetComponent<Console>();
            
            m_mainframe.OnScoreUpdated.AddListener(OnScoreUpdated);
        }

        private void OnScoreUpdated(int _score)
        {
            if (m_lastScore < _score)
            {
                var diff = Mathf.Clamp((int)((_score - m_lastScore) * m_linesMult), m_minLines, m_maxLines);
                for (var i = 0; i < diff; i++)
                {
                    m_console.AddLine(m_commands[Random.Range(0, m_commands.Length)]);
                }
            }
            m_lastScore = _score;
        }
    }
}
