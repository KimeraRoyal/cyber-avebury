using UnityEngine;

public class Hover : MonoBehaviour
{
    [SerializeField] private Vector3 m_amount = Vector3.up;
    [SerializeField] private float m_speed = 1.0f;

    [SerializeField] private float m_minSpeedFactor = 1.0f;
    [SerializeField] private float m_maxSpeedFactor = 1.0f;

    private Vector3 m_anchor;

    private float m_speedFactor;

    private void Awake()
    {
        m_anchor = transform.localPosition;
        
        m_speedFactor = Random.Range(m_minSpeedFactor, m_maxSpeedFactor);
    }

    private void Update()
    {
        transform.localPosition = m_anchor + m_amount * Mathf.Sin(Time.time * m_speed * m_speedFactor);
    }
}
