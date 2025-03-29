using UnityEngine;
using Random = UnityEngine.Random;

public class Throb : MonoBehaviour
{
    [SerializeField] private Vector3 m_minScale = Vector3.one;
    [SerializeField] private Vector3 m_maxScale = Vector3.one;
    
    [SerializeField] private float m_speed = 1.0f;
    [SerializeField] private float m_multiplier = 1.0f;

    [SerializeField] private float m_minSpeedFactor = 1.0f;
    [SerializeField] private float m_maxSpeedFactor = 1.0f;

    private float m_offset;

    private float m_speedFactor;

    private void Awake()
    {
        m_offset = Random.Range(0.0f, Mathf.PI);
        m_speedFactor = Random.Range(m_minSpeedFactor, m_maxSpeedFactor);
    }

    private void Update()
    {
        var t = Mathf.Sin(Time.time * m_speed * m_speedFactor + m_offset) * 0.5f + 0.5f;
        transform.localScale = Vector3.Lerp(m_minScale, m_maxScale, t) * m_multiplier;
    }
}
