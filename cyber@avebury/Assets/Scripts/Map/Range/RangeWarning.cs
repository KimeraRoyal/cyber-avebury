using UnityEngine;

namespace CyberAvebury
{
    [RequireComponent(typeof(CanvasGroup))]
    public class RangeWarning : MonoBehaviour
    {
        private MapRange m_range;

        private CanvasGroup m_group;

        [SerializeField] private Transform m_player;

        [SerializeField] private float m_minDistance = 5.0f;
        [SerializeField] private float m_maxDistance = 30.0f;

        private void Awake()
        {
            m_range = GetComponentInParent<MapRange>();

            m_group = GetComponent<CanvasGroup>();
        }

        private void Update()
        {
            var offset = m_player.position - m_range.transform.position;
            var direction = offset.normalized;
            
            transform.forward = direction;
            transform.position = transform.forward * m_range.Range;
            
            var distance = Vector3.Distance(transform.position, offset);
            m_group.alpha = 1.0f - Mathf.Clamp01((distance - m_minDistance) / (m_maxDistance - m_minDistance));
        }
    }
}
