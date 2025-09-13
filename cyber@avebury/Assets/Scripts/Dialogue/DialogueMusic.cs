using System;
using UnityEngine;

namespace CyberAvebury
{
    [RequireComponent(typeof(Dialogue))]
    public class DialogueMusic : MonoBehaviour
    {
        private Dialogue m_dialogue;

        private void Awake()
        {
            m_dialogue = GetComponent<Dialogue>();

            m_dialogue.OnNewLine.AddListener(OnDialogueLine);
        }

        private void OnDialogueLine(DialogueLineBase _line, int _lineIndex)
        {
            var content = _line.GetContent(_lineIndex);
            if(content == null) { return; }
            
            MusicPlayer.Instance.ChangeMusicState(content.Music);
        }
    }
}
