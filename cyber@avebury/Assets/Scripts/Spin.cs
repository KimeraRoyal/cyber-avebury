using UnityEngine;

public class Spin : MonoBehaviour
{
    [SerializeField] private Vector3 m_speed = Vector3.up;

    [SerializeField] private bool m_randomStart;

    [SerializeField] private float m_minSpeedFactor = 1.0f;
    [SerializeField] private float m_maxSpeedFactor = 1.0f;

    private float m_speedFactor;

    private void Awake()
    {
        m_speedFactor = Random.Range(m_minSpeedFactor, m_maxSpeedFactor);

        if (m_randomStart)
        {
            var startingAngles = new Vector3();
            for (var i = 0; i < 3; i++)
            {
                startingAngles[i] = transform.eulerAngles[i] + m_speed[i] * Random.Range(0.0f, 360.0f);
            }
            transform.eulerAngles = startingAngles;
        }
    }

    private void Update()
    {
        transform.Rotate(m_speed * (m_speedFactor * Time.deltaTime));
    }
}
