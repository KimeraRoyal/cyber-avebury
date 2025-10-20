using System;
using TouchScript.Gestures;
using UnityEngine;
using UnityEngine.UI;

namespace CyberAvebury
{
    [RequireComponent(typeof(Button), typeof(TapGesture))]
    public class MinigameButton : MonoBehaviour
    {
        private Dialogue m_dialogue;
        
        private NodeSelection m_selection;
        
        private Button m_button;
        private TapGesture m_tap;

        private void Awake()
        {
            m_dialogue = FindAnyObjectByType<Dialogue>();
            
            m_selection = FindAnyObjectByType<NodeSelection>();
            m_selection.OnNodeSelected.AddListener(OnNodeSelected);

            m_button = GetComponent<Button>();
            m_tap = GetComponent<TapGesture>();
            m_tap.Tapped += OnTapped;
        }

        private void OnTapped(object _sender, EventArgs _e)
        {
            ClickButton();
        }

        private void OnNodeSelected(Node _node)
        {
            m_button.interactable = _node;
        }

        private void ClickButton()
        {
            if(m_dialogue.HasDialogue) { return; }
            m_selection.SelectedNode.Enter();
            m_selection.LoadSelectedMinigame();
        }
    }
}
