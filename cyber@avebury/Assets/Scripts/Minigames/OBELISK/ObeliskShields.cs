using UnityEngine;

namespace CyberAvebury
{
    public class ObeliskShields : MonoBehaviour
    {
        private Obelisk m_obelisk;
        
        [SerializeField] private ObeliskShield m_shieldPrefab;
        [SerializeField] private float m_shieldDistance = 1.0f;

        private ObeliskShield[] m_shields;

        private void Awake()
        {
            m_obelisk = GetComponentInParent<Obelisk>();
            m_obelisk.OnSubgamesAssigned.AddListener(SpawnShields);
        }

        private void SpawnShields(int _count)
        {
            if(m_shields != null) { return; }
            
            m_shields = new ObeliskShield[_count];
            
            for (var i = 0; i < _count; i++)
            {
                var angle = (Mathf.PI * 2.0f / _count) * i;
                var x = Mathf.Cos(angle) * m_shieldDistance;
                var y = Mathf.Sin(angle) * m_shieldDistance;
                
                m_shields[i] = Instantiate(m_shieldPrefab, new Vector3(x, y, 0.0f), Quaternion.identity, transform);
                m_shields[i].Index = i;
            }
        }
    }
}
