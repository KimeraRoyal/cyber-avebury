using System.Collections.Generic;

namespace CyberAvebury
{
    public class SaveData
    {
        public readonly Dictionary<string, int> DialogueUsage = new();

        public readonly Dictionary<string, NodeState> NodeStates = new();
    }
}