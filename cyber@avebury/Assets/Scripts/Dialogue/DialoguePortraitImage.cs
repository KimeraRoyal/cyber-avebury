using UnityEngine;
using UnityEngine.UI;

namespace CyberAvebury
{
    [RequireComponent(typeof(RectTransform), typeof(MaskableGraphic))]
    public class DialoguePortraitImage : MonoBehaviour
    {
        private Dialogue m_dialogue;
        private WordWriter m_writer;

        private RectTransform m_rect;
        private MaskableGraphic m_graphic;

        [SerializeField] private bool m_changeColor = true;
        [SerializeField] private bool m_changeSize;
        [SerializeField] private bool m_changeMaskable;

        private Vector2 m_defaultSize;
        private Color m_defaultColor;

        private bool m_newLine;

        private void Awake()
        {
            m_dialogue = GetComponentInParent<Dialogue>();
            m_writer = m_dialogue.GetComponentInChildren<WordWriter>();
            
            m_rect = GetComponent<RectTransform>();
            m_defaultSize = m_rect.sizeDelta;
            
            m_graphic = GetComponent<MaskableGraphic>();
            m_defaultColor = m_graphic.color;
            
            m_dialogue.OnNewDialogue.AddListener(OnNewDialogue);
            m_dialogue.OnEndDialogue.AddListener(OnEndDialogue);
            
            m_writer.OnLineStarted.AddListener(OnLineStarted);
            m_writer.OnWordWritten.AddListener(OnWordWritten);
        }

        private void OnNewDialogue(DialogueLineBase _dialogue)
        {
            var character = m_dialogue.CurrentDialogue.GetCharacter(m_dialogue.CurrentLineIndex);
            
            if (m_changeColor) { m_graphic.color = character.PortraitColor; }
        }

        private void OnEndDialogue()
        {
            if (m_changeSize) { m_rect.sizeDelta = m_defaultSize; }
            if (m_changeMaskable) { m_graphic.maskable = true; }
        }

        private void OnLineStarted(string _line)
        {
            var character = m_dialogue.CurrentDialogue.GetCharacter(m_dialogue.CurrentLineIndex);
            
            if (m_changeColor) { m_graphic.color = character.PortraitColor; }
            if (m_changeSize) { m_rect.sizeDelta = m_defaultSize; }
            if (m_changeMaskable) { m_graphic.maskable = true; }

            m_newLine = true;
        }

        private void OnWordWritten(string _word)
        {
            if(!m_newLine) { return; }

            var character = m_dialogue.CurrentDialogue.GetCharacter(m_dialogue.CurrentLineIndex);
            
            if(m_changeSize) { m_rect.sizeDelta = character.PortraitSize; }
            if(m_changeMaskable) { m_graphic.maskable = character.PortraitMaskable; }
            
            m_newLine = false;
        }
    }
}