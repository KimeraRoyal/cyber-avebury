using CyberAvebury.Minigames;
using UnityEngine;
using UnityEngine.Events;

namespace CyberAvebury
{
    public class Boss : MonoBehaviour
    {
        private Minigame m_minigame;
        private Antivirus m_antivirus;
        
        private ProjectileTarget m_target;

        [SerializeField] private int m_hitsPerScore = 10;
        private int m_currentHits;

        [SerializeField] private Vector3 m_defeatedMovementSpeed;
        private bool m_defeated;
        
        [SerializeField] private float m_movementDuration = 10.0f;
        private float m_movementTimer;
        
        [SerializeField] private Vector3 m_startPosition;
        [SerializeField] private Vector3 m_finishPosition;
        [SerializeField] private AnimationCurve m_movementCurve = AnimationCurve.Linear(0, 0, 1, 1);

        public float MovementProgress => Mathf.Clamp(m_movementTimer / m_movementDuration, 0.0f, 1.0f);

        public UnityEvent<float> OnMove;
        public UnityEvent OnFinishMoving;

        public UnityEvent OnHurt;
        public UnityEvent OnDefeated;

        private void Awake()
        {
            m_minigame = GetComponentInParent<Minigame>();
            m_antivirus = m_minigame.GetComponent<Antivirus>();
            
            m_minigame.OnPassed.AddListener(OnMinigamePassed);
            
            m_target = GetComponentInChildren<ProjectileTarget>();
            m_target.Hittable = true;
            m_target.OnHit.AddListener(OnHit);
        }

        private void Update()
        {
            if (m_defeated)
            {
                transform.localPosition += m_defeatedMovementSpeed * Time.deltaTime;
                return;
            }
            
            if(!m_minigame.IsPlaying) { return; }
            
            m_movementTimer += Time.deltaTime;
            transform.localPosition = Vector3.Lerp(m_startPosition, m_finishPosition, m_movementCurve.Evaluate(MovementProgress));
            OnMove?.Invoke(MovementProgress);
            
            if(m_movementTimer < m_movementDuration) { return; }
            
            OnFinishMoving?.Invoke();
            m_minigame.Fail();
        }

        private void OnHit(Projectile _projectile)
        {
            m_currentHits++;
            if(m_currentHits < m_hitsPerScore) { return; }
            m_currentHits = 0;
            
            m_antivirus.ChangeScore(1);
            OnHurt?.Invoke();
        }

        private void OnMinigamePassed()
        {
            m_defeated = true;
            OnDefeated?.Invoke();
        }
    }
}
