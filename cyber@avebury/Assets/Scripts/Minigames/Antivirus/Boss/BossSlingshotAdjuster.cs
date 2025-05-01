using UnityEngine;

namespace CyberAvebury
{
    public class BossSlingshotAdjuster : MonoBehaviour
    {
        private Slingshot m_slingshot;
        
        private Boss m_boss;
        
        [SerializeField] private Vector3 m_startDistanceFactor;
        [SerializeField] private Vector3 m_finishDistanceFactor;
        [SerializeField] private AnimationCurve m_distanceFactorCurve = AnimationCurve.Linear(0, 0, 1, 1);
        [SerializeField] private float m_distanceFactorSmoothTime = 0.5f;
        private Vector3 m_distanceFactorVelocity;

        private void Awake()
        {
            m_slingshot = FindAnyObjectByType<Slingshot>();

            m_boss = GetComponentInParent<Boss>();
            
            m_boss.OnMove.AddListener(OnBossMove);
        }

        private void OnBossMove(float _progress)
        {
            var target = Vector3.Lerp(m_startDistanceFactor, m_finishDistanceFactor, m_distanceFactorCurve.Evaluate(_progress));
            m_slingshot.DistanceFactor = Vector3.SmoothDamp(m_slingshot.DistanceFactor, target, ref m_distanceFactorVelocity, m_distanceFactorSmoothTime);
        }
    }
}
