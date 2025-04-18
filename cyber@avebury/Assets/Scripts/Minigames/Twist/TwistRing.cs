using CyberAvebury.Minigames;
using UnityEngine;
using UnityEngine.Events;

namespace CyberAvebury
{
    public class TwistRing : MonoBehaviour
    {
        private Minigame m_minigame;

        [SerializeField] private DifficultyAdjustedFloat m_degreeRangeDifficulty = new(60.0f, 30.0f);
        [SerializeField] private DifficultyAdjustedFloat m_spinSpeedDifficulty = new(2.0f, 20.0f);

        private float m_degreeRange;
        private float m_spinSpeed;

        private bool m_active;

        public float DegreeRange => m_degreeRange;
        public float SpinSpeed => m_spinSpeed;

        public bool IsAngleValid
        {
            get
            {
                var angle = transform.localEulerAngles.z;
                var targetAngle = angle > 180.0f ? 360.0f : 0.0f;
                var difference = Mathf.Abs(targetAngle - angle);
                return difference <= m_degreeRange / 2;
            }
        }

        public bool IsActive
        {
            get { return m_active; }
            set
            {
                if(m_active == value) { return; }
                m_active = value;
                OnActiveChanged?.Invoke(m_active);
            }
        }

        public UnityEvent<float> OnDegreeRangeChanged;

        public UnityEvent<bool> OnActiveChanged;

        private void Awake()
        {
            m_minigame = GetComponentInParent<Minigame>();
            m_minigame.OnDifficultySet.AddListener(OnDifficultySet);
        }

        private void Start()
        {
            var eulerAngles = transform.localEulerAngles;
            eulerAngles.z = Random.Range(0, 360);
            transform.localEulerAngles = eulerAngles;
        }

        private void Update()
        {
            if(!IsActive) { return; }

            var eulerAngles = transform.localEulerAngles;
            eulerAngles.z += Time.deltaTime * m_spinSpeed;
            transform.localEulerAngles = eulerAngles;
        }

        private void OnDifficultySet(float _difficulty)
        {
            m_degreeRange = m_degreeRangeDifficulty.GetValue(_difficulty);
            OnDegreeRangeChanged?.Invoke(m_degreeRange);

            m_spinSpeed = m_spinSpeedDifficulty.GetValue(_difficulty) * (Random.Range(0, 2) * 2 - 1);
        }
    }
}
