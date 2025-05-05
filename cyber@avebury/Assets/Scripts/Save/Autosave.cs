using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace CyberAvebury
{
    public class Autosave : MonoBehaviour
    {
        [SerializeField] private float m_saveDelay = 1.0f;
        [SerializeField] private float m_loadingBlockDuration = 0.1f;

        private bool m_saving;
        private bool m_blocking;

        public UnityEvent OnAutosave;

        private void Awake()
        {
            Saving.Instance.OnLoad.AddListener(OnSaveLoaded);
        }

        public void Save()
        {
            if(m_saving || m_blocking) { return; }
            
            m_saving = true;
            StartCoroutine(WaitAndSave());
        }

        private IEnumerator WaitAndSave()
        {
            yield return new WaitForSeconds(m_saveDelay);
            
            Saving.Instance.Save();
            OnAutosave?.Invoke();
            
            m_saving = false;
        }

        private void OnSaveLoaded(SaveData _data)
        {
            m_blocking = true;
            StartCoroutine(BlockSaving());
        }

        private IEnumerator BlockSaving()
        {
            yield return new WaitForSeconds(m_loadingBlockDuration);
            m_blocking = false;
        }
    }
}
