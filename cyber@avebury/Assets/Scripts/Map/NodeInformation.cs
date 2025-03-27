using System;
using Niantic.Lightship.Maps.Core.Coordinates;
using UnityEngine;

namespace CyberAvebury
{
    [Serializable]
    public class NodeInformation
    {
        [SerializeField] private string m_name = "Node";
        
        [SerializeField] private double m_longitude;
        [SerializeField] private double m_latitude;

        public string Name => m_name;

        public LatLng Coordinates => new LatLng(m_longitude, m_latitude);
    }
}