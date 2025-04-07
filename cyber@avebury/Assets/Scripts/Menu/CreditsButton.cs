using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace CyberAvebury
{
    [RequireComponent(typeof(Button))]
    public class CreditsButton : MonoBehaviour
    {
        private MenuScreens m_screens;
        
        [SerializeField] private Console m_menuConsole;

        private Button m_button;

        [SerializeField] private string m_command;

        private void Awake()
        {
            m_screens = FindAnyObjectByType<MenuScreens>();
            
            m_button = GetComponent<Button>();
            m_button.onClick.AddListener(OnButtonPressed);
        }

        public void OnButtonPressed()
        {
            m_menuConsole.ReturnBehaviour = ReturnBehaviour.EndOfLine;
            m_menuConsole.AddLine(m_command);

            m_screens.ChangeState(MenuScreens.MenuState.Credits);
        }
    }
}
