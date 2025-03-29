using UnityEngine;

public class Spin : MonoBehaviour
{
    [SerializeField] private Vector3 m_speed = Vector3.up;

    [SerializeField] private float m_minSpeedFactor = 1.0f;
    [SerializeField] private float m_maxSpeedFactor = 1.0f;

    private float m_speedFactor;

    private void Awake()
    {
        m_speedFactor = Random.Range(m_minSpeedFactor, m_maxSpeedFactor);
    }

    private void Update()
    {
        transform.Rotate(m_speed * (m_speedFactor * Time.deltaTime));
    }
}
