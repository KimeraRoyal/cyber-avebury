using UnityEngine;

public class PlaceObjectAtRay : MonoBehaviour
{
    [SerializeField] private Transform m_target;

    [SerializeField] private float m_yPlane;

    private void Update()
    {
        var planePosition = Vector3.up * m_yPlane;
        var planeNormal = -Vector3.up;

        var denominator = Vector3.Dot(transform.forward, planeNormal);
        if(denominator < 0.00001f) { return; }

        var distance = Vector3.Dot(planePosition - transform.position, planeNormal) / denominator;
        var intersection = transform.position + transform.forward * distance;

        m_target.position = intersection;
    }
}
