﻿using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace CyberAvebury
{
    [Serializable]
    public abstract class DialogueLineBase
    {
        [SerializeField] private int m_maxUses = -1;
        [SerializeField] private int m_uses;

        [SerializeField] private bool m_shouldLoadNextScene;
        [SerializeField] private int m_loadSceneIndex = -1;

        public bool CanUse => m_maxUses < 0 || m_uses < m_maxUses;
        public int MaxUses => m_maxUses;

        public int Uses
        {
            get => m_uses;
            set => m_uses = value;
        }

        public bool ShouldLoadNextScene => m_shouldLoadNextScene;
        public int LoadSceneIndex => m_loadSceneIndex;
        
        public abstract DialogueLineContent GetContent(int _lineIndex);
        
        public DialogueCharacter GetCharacter(int _lineIndex)
            => GetContent(_lineIndex).Character;
        public int GetExpressionIndex(int _lineIndex)
            => GetContent(_lineIndex).Expression;
        public string GetLine(int _lineIndex)
            => GetContent(_lineIndex).Line;
        
        public abstract int GetLineCount();

        public void ReportUse()
            => m_uses++;

        public void ResetUses()
            => m_uses = 0;
    }
}