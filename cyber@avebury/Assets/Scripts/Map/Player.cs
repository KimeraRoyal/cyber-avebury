using Niantic.Lightship.Maps;
using UnityEngine;

namespace CyberAvebury
{
    public class Player : MonoBehaviour
    {
        private LightshipMapView m_view;

        [SerializeField] private Transform m_target;

        private void Awake()
        {
            m_view = FindAnyObjectByType<LightshipMapView>();
        }

        public float GetDistanceToPoint(Vector3 _point)
        {
            var distance = Vector3.Distance(transform.position, _point);
            var latLng = m_view.SceneToLatLng(transform.position);

            return (float) m_view.SceneToMeters(distance, latLng.Latitude);
        }
    }
}
