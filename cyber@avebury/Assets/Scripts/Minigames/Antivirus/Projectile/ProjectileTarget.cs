using UnityEngine;
using UnityEngine.Events;

namespace CyberAvebury
{
    public class ProjectileTarget : MonoBehaviour
    {
        private bool m_hittable;
        
        public UnityEvent<Projectile> OnHit;

        public bool Hittable
        {
            get => m_hittable;
            set => m_hittable = value;
        }

        public bool Hit(Projectile _projectile)
        {
            if (!m_hittable) { return false; }
            
            OnHit?.Invoke(_projectile);
            return true;
        }
    }
}
