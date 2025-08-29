using System;
using System.Collections.Generic;
using UnityEngine;

namespace CyberAvebury
{
    public class FeaturePlacer : MonoBehaviour
    {
        [Serializable]
        private class PlaceableFeature
        {
            [SerializeField] private LatLng m_coordinates;
            [SerializeField] private Feature m_featurePrefab;

            public Feature Place(GPS _gps, Transform _parent)
            {
                var feature = Instantiate(m_featurePrefab, _gps.GetScenePosition(m_coordinates), Quaternion.identity, _parent);
                return feature;
            }
        }

        private GPS m_gps;
        
        [SerializeField] private PlaceableFeature[] m_features;

        private List<Feature> m_placedFeatures;

        public IReadOnlyList<Feature> PlacedFeatures => m_placedFeatures;

        private void Awake()
        {
            m_gps = FindAnyObjectByType<GPS>();
            
            m_placedFeatures = new List<Feature>();
        }

        private void Start()
        {
            foreach (var feature in m_features)
            {
                var placedFeature = feature.Place(m_gps, transform);
                m_placedFeatures.Add(placedFeature);
            }
        }
    }
}
