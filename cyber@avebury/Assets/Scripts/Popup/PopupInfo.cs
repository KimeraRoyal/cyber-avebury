using UnityEngine;

namespace CyberAvebury
{
    [CreateAssetMenu(fileName = "Popup", menuName = "cyber@avebury/Popup")]
    public class PopupInfo : ScriptableObject
    {
        [SerializeField] private string m_title = "Popup";
        [SerializeField] private Sprite m_image;
        [SerializeField] [TextArea(3, 5)] private string m_description = "Description";

        [SerializeField] private DialogueLineObjectBase m_onClosedDialogue;
        [SerializeField] private PopupInfo m_nextPopup;

        public string Title => m_title;
        public Sprite Image => m_image;
        public string Description => m_description;

        public DialogueLineObjectBase OnClosedDialogue => m_onClosedDialogue;
        public PopupInfo NextPopup => m_nextPopup;
    }
}
