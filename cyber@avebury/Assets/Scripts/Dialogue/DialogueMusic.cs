using System;
using UnityEngine;

namespace CyberAvebury
{
    [RequireComponent(typeof(MusicPlayer))]
    public class DialogueMusic : MonoBehaviour
    {
        private Dialogue m_dialogue;

        private MusicPlayer m_music;

        private void Awake()
        {
            m_dialogue = FindAnyObjectByType<Dialogue>();

            m_music = GetComponent<MusicPlayer>();
            
            m_dialogue.OnNewLine.AddListener(OnDialogueLine);
        }

        private void OnDialogueLine(DialogueLineBase _line, int _lineIndex)
        {
            var content = _line.GetContent(_lineIndex);
            if(content == null) { return; }
            
            m_music.PlaySong(content.Music);
        }
    }
}
