using UnityEngine;

public class Spin : MonoBehaviour
{
    [SerializeField] private Vector3 m_speed = Vector3.up;

    private void Update()
    {
        transform.Rotate(m_speed * Time.deltaTime);
    }
}
