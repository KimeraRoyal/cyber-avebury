using UnityEngine;

namespace CyberAvebury
{
    public class MapRange : MonoBehaviour
    {
        private GPS m_gps;
        
        [SerializeField] private float m_range = 10.0f;

        public float Range => m_gps ? m_range * m_gps.MapScale : m_range;

        private void Awake()
        {
            m_gps = FindAnyObjectByType<GPS>();
        }

        public bool IsInRange(Vector3 _worldPosition)
            => Vector3.Distance(_worldPosition, transform.position) <= Range;

        public bool IsInRange(LatLng _coordinates)
            => IsInRange(m_gps.GetScenePosition(_coordinates));

        public float OutOfRangeBy(Vector3 _worldPosition)
        {
            if (IsInRange(_worldPosition)) { return 0.0f; }
            var distance = Vector3.Distance(_worldPosition, transform.position);
            return distance - Range;
        }
        
        public float OutOfRangeBy(LatLng _coordinates)
            => OutOfRangeBy(m_gps.GetScenePosition(_coordinates));
    }
}
