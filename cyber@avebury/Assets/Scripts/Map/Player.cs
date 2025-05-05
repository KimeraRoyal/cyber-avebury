using Niantic.Lightship.Maps;
using UnityEngine;

namespace CyberAvebury
{
    public class Player : MonoBehaviour
    {
        private LightshipMapView m_view;

        private void Awake()
        {
            m_view = FindAnyObjectByType<LightshipMapView>();
        }

        public float GetDistanceToPoint(Vector3 _point)
        {
            var distance = Vector2.Distance(GetFlatPosition(), new Vector2(_point.x, _point.z));
            return SceneToMeters(distance);
        }

        public float SceneToMeters(float _value)
        {
            var latLng = m_view.SceneToLatLng(transform.position);
            return (float) m_view.SceneToMeters(_value, latLng.Latitude);
        }

        public float MetersToScene(float _value)
        {
            var latLng = m_view.SceneToLatLng(transform.position);
            return (float) m_view.MetersToScene(_value, latLng.Latitude);
        }

        public Vector2 GetFlatPosition()
            => new (transform.position.x, transform.position.z);
    }
}
