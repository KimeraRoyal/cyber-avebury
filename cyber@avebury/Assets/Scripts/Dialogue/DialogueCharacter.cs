using UnityEngine;

namespace CyberAvebury
{
    [CreateAssetMenu(fileName = "Character", menuName = "cyber@avebury/Character")]
    public class DialogueCharacter : ScriptableObject
    {
        [SerializeField] private Sprite m_portrait;

        [SerializeField] private float m_letterDuration = 0.1f;

        public Sprite Portrait => m_portrait;

        public float LetterDuration => m_letterDuration;
    }
}