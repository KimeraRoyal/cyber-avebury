using UnityEngine;
using UnityEngine.UI;

namespace CyberAvebury
{
    public class DialogueNoise : MonoBehaviour
    {
        private Dialogue m_dialogue;
        private WordWriter m_writer;

        private Image m_image;

        private void Awake()
        {
            m_dialogue = GetComponentInParent<Dialogue>();
            m_writer = m_dialogue.GetComponentInChildren<WordWriter>();
            
            m_image = GetComponent<Image>();
            
            m_dialogue.OnNewDialogue.AddListener(_ => ShowStatic());
            m_dialogue.OnEndDialogue.AddListener(ShowStatic);
            
            m_writer.OnLineStarted.AddListener(_ => ShowStatic());
            m_writer.OnWordWritten.AddListener(_ => HideStatic());
            m_writer.OnLineFinished.AddListener(_ => HideStatic());
        }

        private void ShowStatic()
            => m_image.enabled = true;

        private void HideStatic()
            => m_image.enabled = false;
    }
}
