using UnityEngine;

namespace CyberAvebury
{
    public class GPS : MonoBehaviour
    {
        [SerializeField] private LatLng m_origin;
        [SerializeField] private float m_mapScale = 1.0f;

        public LatLng Origin => m_origin;

        private void Awake()
        {
            GPSEncoder.SetLocalOrigin(m_origin);
        }
        
        public T PlaceObjectAt<T>(T _prefab, LatLng _coordinates, Quaternion _rotation) where T : Object
            => Instantiate(_prefab, GetScenePosition(_coordinates), _rotation, transform);

        public Vector3 GetScenePosition(LatLng _coordinates)
            => MetersToScene(GPSEncoder.GPSToUCS(_coordinates));

        public LatLng GetCoordinates(Vector3 _position)
            => GPSEncoder.USCToGPS(SceneToMeters(_position));

        #region Unit Conversion
        public float SceneToMeters(float _sceneUnits)
            => _sceneUnits / m_mapScale;

        public Vector2 SceneToMeters(Vector2 _sceneUnits)
            => _sceneUnits / m_mapScale;

        public Vector3 SceneToMeters(Vector3 _sceneUnits)
            => _sceneUnits / m_mapScale;

        public float MetersToScene(float _unitInMeters)
            => _unitInMeters * m_mapScale;

        public Vector2 MetersToScene(Vector2 _unitInMeters)
            => _unitInMeters * m_mapScale;

        public Vector3 MetersToScene(Vector3 _unitInMeters)
            => _unitInMeters * m_mapScale;
        #endregion
    }
}