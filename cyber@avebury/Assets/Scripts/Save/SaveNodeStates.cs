using System.Collections;
using UnityEngine;

namespace CyberAvebury
{
    public class SaveNodeStates : MonoBehaviour
    {
        private Nodes m_nodes;
        
        private Nodes Nodes => m_nodes ??= FindAnyObjectByType<Nodes>();

        private Coroutine m_coroutine;
        
        private void Awake()
        {
            Saving.Instance.OnSave.AddListener(Save);
            Saving.Instance.OnLoad.AddListener(Load);
        }

        private void Start()
        {
            if(Saving.Instance.HasSaveData) { Load(Saving.Instance.SaveData); }
        }

        private void Save(SaveData _saveData)
        {
            foreach (var node in Nodes.NodeList)
            {
                _saveData.NodeStates[node.Info.name] = node.CurrentState;
            }
        }

        private void Load(SaveData _saveData)
        {
            if (m_coroutine != null)
            {
                StopCoroutine(m_coroutine);
                m_coroutine = null;
            }
            m_coroutine = StartCoroutine(LoadWhenReady(_saveData));
        }

        private IEnumerator LoadWhenReady(SaveData _saveData)
        {
            yield return new WaitUntil(() => Nodes.RegistrationComplete);
            foreach (var node in Nodes.NodeList)
            {
                if(!_saveData.NodeStates.TryGetValue(node.Info.name, out var state)) { continue; }
                node.CurrentState = state;
            }
        }
    }
}