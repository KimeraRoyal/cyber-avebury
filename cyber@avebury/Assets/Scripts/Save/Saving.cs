using System.IO;
using Sirenix.OdinInspector;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Events;

namespace CyberAvebury
{
    public class Saving : MonoBehaviour
    {
        private static Saving s_instance;
        public static Saving Instance
        {
            get
            {
                if(!s_instance) { s_instance = FindAnyObjectByType<Saving>(); }
                return s_instance;
            }
            private set => s_instance = value;
        }

        [SerializeField] private string m_saveFileName = "save.json";
        [SerializeField] [TextArea(3, 10)] private string m_defaultSave;

        private SaveData m_saveData;

        public SaveData SaveData
        {
            get => m_saveData ??= new SaveData();
            private set => m_saveData = value;
        }

        public bool HasSaveData => m_saveData != null;

        public UnityEvent<SaveData> OnSave;
        public UnityEvent<SaveData> OnLoad;

        private void Awake()
        {
            if (Instance && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            DontDestroyOnLoad(gameObject);
            Instance = this;
        }
        
        [Button("Save to File")]
        public void Save()
        {
            OnSave?.Invoke(SaveData);
            WriteToFile(JsonConvert.SerializeObject(SaveData), m_saveFileName);
        }

        [Button("Load from File")]
        public void Load()
            => LoadFromString(ReadFromFile(m_saveFileName));

        [Button("Reset to Default")]
        public void ResetToDefault()
            => LoadFromString(m_defaultSave);

        private void LoadFromString(string _data)
        {
            SaveData = JsonConvert.DeserializeObject<SaveData>(_data);
            OnLoad?.Invoke(SaveData);
        }

        private static void WriteToFile(string _data, string _localPath)
        {
            var path = GetFilePath(_localPath);
            if (!Directory.Exists(Path.GetDirectoryName(path)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(path));
            }
            
            if (Debug.isDebugBuild) { Debug.Log($"Writing to File: {path}"); }
            File.WriteAllText(path, _data);
        }

        private static string ReadFromFile(string _localPath)
        {
            var path = GetFilePath(_localPath);
            if (!File.Exists(path))
            {
                return "";
            }

            if (Debug.isDebugBuild) { Debug.Log($"Reading from File: {path}"); }
            return File.ReadAllText(path);
        }

        private static string GetFilePath(string _localPath)
            => Path.GetFullPath(Path.Combine(Application.persistentDataPath, _localPath));
    }
}
