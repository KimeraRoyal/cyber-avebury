using UnityEditor;
using UnityEngine;

namespace CyberAvebury
{
    [CustomEditor(typeof(MapRange))]
    public class MapRangeEditor : Editor
    {
        private void OnSceneGUI()
        {
            var range = target as MapRange;
            if(!range) { return; }
            
            Handles.color = new Color(0.0f, 0.75f, 0.0f, .2f);
            Handles.DrawSolidDisc(range.transform.position,
                range.transform.up,
                range.Range);
            
            Handles.color = new Color(0.0f, 1.0f, 0.0f, 1.0f);
            Handles.DrawWireDisc(
                range.transform.position,
                range.transform.up,
                range.Range);
        }
    }
}
