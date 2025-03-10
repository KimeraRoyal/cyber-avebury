using CyberAvebury.Minigame.Mainframe.Rings;
using UnityEditor;
using UnityEngine;

namespace CyberAvebury.Minigame.Mainframe.Editor
{
    [CustomEditor(typeof(RingArea))]
    public class RingAreaEditor : UnityEditor.Editor
    {
        private void OnSceneGUI()
        {
            var ringArea = target as RingArea;
            if(!ringArea) { return; }

            var edgeBufferSize = ringArea.EdgeBufferSize;
            var minBound = ringArea.CalculateMinBound();
            var maxBound = ringArea.CalculateMaxBound();

            if (edgeBufferSize.magnitude > 0.001f)
            {
                var areaSize = (maxBound + ringArea.EdgeBufferSize) - (minBound - ringArea.EdgeBufferSize);
                Handles.color = Color.red;
                Handles.DrawWireCube(ringArea.transform.position, areaSize);
            }
            
            var validAreaSize = maxBound - minBound;
            Handles.color = Color.green;
            Handles.DrawWireCube(ringArea.transform.position, validAreaSize);
        }
    }
}
