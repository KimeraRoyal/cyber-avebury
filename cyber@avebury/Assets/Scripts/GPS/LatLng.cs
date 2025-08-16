using System;
using UnityEngine;

namespace CyberAvebury
{
    [Serializable]
    public struct LatLng
    {
        [SerializeField] private double m_latitude;
        [SerializeField] private double m_longitude;
        
        public double Latitude { get => m_latitude; set => m_latitude = value; }
        public double Longitude { get => m_longitude; set => m_longitude = value; }

        public LatLng(double _latitude, double _longitude)
        {
            m_latitude = _latitude;
            m_longitude = _longitude;
        }
            
        public static implicit operator Vector2(LatLng _coordinates)
            => new((float) _coordinates.Latitude, (float) _coordinates.Longitude);

        public static implicit operator LatLng(Vector2 _position)
            => new(_position.x, _position.y);
    }
}