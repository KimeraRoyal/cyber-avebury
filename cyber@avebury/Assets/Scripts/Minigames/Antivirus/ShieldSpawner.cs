using UnityEngine;

namespace CyberAvebury
{
    [RequireComponent(typeof(RectTransform))]
    public class ShieldSpawner : MonoBehaviour
    {
        private Antivirus m_antivirus;
        
        private RectTransform m_rect;
        
        [SerializeField] private Shield m_shieldPrefab;
        [SerializeField] private float m_maxDistance = 64;

        private void Awake()
        {
            m_antivirus = GetComponentInParent<Antivirus>();
            m_antivirus.OnTargetScoreInitialized.AddListener(SpawnShields);

            m_rect = GetComponent<RectTransform>();
        }

        private void SpawnShields(int _health)
        {
            var distance = m_rect.rect.width / (_health - 1);
            distance = Mathf.Min(distance, m_maxDistance);

            var totalDistance = distance * (_health - 1);
            var offset = -totalDistance / 2.0f;
            
            for (var i = 0; i < _health; i++)
            {
                var shield = Instantiate(m_shieldPrefab, m_rect);
                shield.Antivirus = m_antivirus;
                shield.Position = new Vector2(distance * i + offset, 0.0f);
                shield.Index = i;
            }
        }
    }
}
