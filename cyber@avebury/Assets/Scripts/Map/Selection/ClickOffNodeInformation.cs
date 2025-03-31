using UnityEngine;
using UnityEngine.EventSystems;

namespace CyberAvebury
{
    // I WANTED TO JUST USE AN EVENT TRIGGER FOR THIS BUT THAT MAKES UNITY ERROR.
    // SO BLAME UNITY FOR THIS CLASS EXISTING
    public class ClickOffNodeInformation : MonoBehaviour, IPointerDownHandler
    {
        private NodeSelection m_selection;

        private void Awake()
        {
            m_selection = FindAnyObjectByType<NodeSelection>();
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            m_selection.SelectNode(null);
        }
    }
}
