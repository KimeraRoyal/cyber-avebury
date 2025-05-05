using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace CyberAvebury
{
    [RequireComponent(typeof(Button))]
    public class LoadButton : MonoBehaviour
    {
        private enum Behaviour
        {
            Load,
            Reset
        }

        private Button m_button;

        [SerializeField] private Behaviour m_behaviour;
        private bool m_primed;

        private void Awake()
        {
            m_button = GetComponent<Button>();
            m_button.onClick.AddListener(OnClick);
            
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void Start()
        {
            if(m_behaviour != Behaviour.Load) { return; }
            m_button.interactable = Saving.Instance.SaveExistsOnDisk;
        }

        private void OnClick()
        {
            m_primed = true;
            LoadingScreen.Instance.ShowScreen(1.0f, () => SceneManager.LoadScene(1));
        }

        private void OnSceneLoaded(Scene _scene, LoadSceneMode _mode)
        {
            if(!m_primed) { return; }
            m_primed = false;
            
            switch (m_behaviour)
            {
                case Behaviour.Load:
                    Saving.Instance.Load();
                    break;
                case Behaviour.Reset:
                    Saving.Instance.ResetToDefault();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
