using UnityEngine;

namespace CyberAvebury
{
    public class WordGraphicPool : MonoBehaviour
    {
        [SerializeField] private WordGraphic m_prefab;

        private void Start()
        {
            var words = FindObjectsByType<Word>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);
            foreach (var word in words)
            {
                OnWordCreated(word);
            }
        }

        private WordGraphic Get()
        {
            return Instantiate(m_prefab, transform);
        }

        private void OnWordCreated(Word _word)
        {
            var graphic = Get();
            graphic.Word = _word;
        }
    }
}
