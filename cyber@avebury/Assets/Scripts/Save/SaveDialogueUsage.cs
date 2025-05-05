using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace CyberAvebury
{
    public class SaveDialogueUsage : MonoBehaviour
    {
        [SerializeField] private DialogueLineObjectBase[] m_allLines;
        
        private uint m_lastSaveCount;
        
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
            if(m_allLines == null) { return; }
            foreach (var line in m_allLines)
            {
                _saveData.DialogueUsage[line.name] = line.GetLine().Uses;
            }
        }

        private void Load(SaveData _saveData)
        {
            if(m_allLines == null) { return; }

            foreach (var line in m_allLines)
            {
                if(!_saveData.DialogueUsage.TryGetValue(line.name, out var uses)) { continue; }
                line.GetLine().Uses = uses;
            }
        }

        [Button("Find Dialogue in Files")]
        private void FindDialogueInFiles()
        {
#if UNITY_EDITOR
            var guids = AssetDatabase.FindAssets("t:"+ nameof(DialogueLineObjectBase));
            var assets = new DialogueLineObjectBase[guids.Length];
            for(var i = 0; i < guids.Length; i++)
            {
                var path = AssetDatabase.GUIDToAssetPath(guids[i]);
                assets[i] = AssetDatabase.LoadAssetAtPath<DialogueLineObjectBase>(path);
            }

            m_allLines = assets;
#endif
        }
    }
}