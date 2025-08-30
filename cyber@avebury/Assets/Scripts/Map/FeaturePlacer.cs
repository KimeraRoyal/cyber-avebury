using System;
using System.Collections.Generic;
using UnityEngine;

namespace CyberAvebury
{
    public class FeaturePlacer : MonoBehaviour
    {
        [Serializable]
        public class FeatureInfo
        {
            [SerializeField] private string m_coordinates;
            [SerializeField] private float m_rotation;

            public string Coordinates => m_coordinates;
            public float Rotation => m_rotation;
        }

        [Serializable]
        public class FeatureSet
        {
            [SerializeField] private Feature m_featurePrefab;
            [SerializeField] private FeatureInfo[] m_features;

            public void PlaceFeatures(GPS _gps, List<Feature> _features, Transform _parent)
            {
                foreach (var featureInfo in m_features)
                {
                    if(!LatLng.FromString(featureInfo.Coordinates, out var coordinates)) { continue; }
                    
                    var rotation = Quaternion.Euler(0.0f, -featureInfo.Rotation, 0.0f);
                    var feature = Instantiate(m_featurePrefab, _gps.GetScenePosition(coordinates), rotation, _parent);
                    _features.Add(feature);
                }
            }
        }

        private GPS m_gps;
        
        [SerializeField] private FeatureSet[] m_featureSets;

        private List<Feature> m_placedFeatures;

        public IReadOnlyList<Feature> PlacedFeatures => m_placedFeatures;

        private void Awake()
        {
            m_gps = FindAnyObjectByType<GPS>();
            
            m_placedFeatures = new List<Feature>();
        }

        private void Start()
        {
            foreach (var featureSet in m_featureSets)
            {
                featureSet.PlaceFeatures(m_gps, m_placedFeatures, transform);
            }
        }
    }
}
