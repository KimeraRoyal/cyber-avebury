using UnityEngine;

namespace CyberAvebury
{
    public abstract class DialogueLineObjectBase : ScriptableObject
    {
        public abstract DialogueLineBase GetLine();
    }
}
