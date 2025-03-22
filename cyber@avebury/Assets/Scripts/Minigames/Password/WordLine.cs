using System.Collections.Generic;
using UnityEngine;

namespace CyberAvebury
{
    public class WordLine : MonoBehaviour
    {
        private List<Word> m_words;

        private void Awake()
        {
            m_words = new List<Word>();
            m_words.AddRange(GetComponentsInChildren<Word>());
        }

        public int EvaluatePositionIndex(Vector2 _position)
        {
            var index = 0;
            return index;
        }
    }
}
