using UnityEngine;

namespace CyberAvebury
{
    public class Credits : MonoBehaviour
    {
        private Console m_console;

        [SerializeField] [TextArea(3, 10)] private string m_lines;
        
        private void Awake()
        {
            m_console = GetComponentInChildren<Console>();
        }

        private void OnEnable()
        {
            m_console.AddLine(m_lines);
        }

        private void OnDisable()
        {
            m_console.ClearLines();
        }
    }
}
