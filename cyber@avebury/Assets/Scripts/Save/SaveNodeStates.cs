using UnityEngine;

namespace CyberAvebury
{
    public class SaveNodeStates : MonoBehaviour
    {
        private Nodes m_nodes;
        
        private Nodes Nodes => m_nodes ??= FindAnyObjectByType<Nodes>();
        
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
            foreach (var node in Nodes.NodeList)
            {
                if(!_saveData.NodeStates.TryGetValue(node.Info.name, out var state)) { continue; }
                node.CurrentState = state;
            }
        }
    }
}