using UnityEngine;

namespace CyberAvebury
{
    public class Player : MonoBehaviour
    {
        private GPS m_gps;

        private void Awake()
        {
            m_gps = FindAnyObjectByType<GPS>();
        }

        public float GetDistanceToPoint(Vector3 _point)
        {
            var distance = Vector2.Distance(GetFlatPosition(), new Vector2(_point.x, _point.z));
            return m_gps.SceneToMeters(distance);
        }

        public Vector2 GetFlatPosition()
            => new (transform.position.x, transform.position.z);
    }
}
