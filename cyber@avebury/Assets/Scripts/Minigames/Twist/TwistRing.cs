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

        public float DegreeRange => m_degreeRange;
        public float SpinSpeed => m_spinSpeed;

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
            var eulerAngles = transform.localEulerAngles;
            eulerAngles.z += Time.deltaTime * m_spinSpeed;
            transform.localEulerAngles = eulerAngles;
        }

        private void OnDifficultySet(float _difficulty)
        {
            m_degreeRange = m_degreeRangeDifficulty.GetValue(_difficulty);
            m_spinSpeed = m_spinSpeedDifficulty.GetValue(_difficulty) * (Random.Range(0, 2) * 2 - 1);
        }
    }
}
