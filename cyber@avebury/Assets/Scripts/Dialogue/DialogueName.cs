using TMPro;
using UnityEngine;

namespace CyberAvebury
{
    [RequireComponent(typeof(TMP_Text))]
    public class DialogueName : MonoBehaviour
    {
        private Dialogue m_dialogue;

        private TMP_Text m_text;

        private void Awake()
        {
            m_dialogue = GetComponentInParent<Dialogue>();

            m_text = GetComponent<TMP_Text>();
            
            m_dialogue.OnNewLine.AddListener(UpdateCharacterName);
        }

        private void UpdateCharacterName(DialogueLine _line)
        {
            m_text.text = _line.Character.name.ToUpper();
        }
    }
}
