using System;
using UnityEngine;

namespace CyberAvebury.Minigames
{
    [Serializable]
    public abstract class DifficultyAdjustedValue<T>
    {
        [SerializeField] private T m_easiestValue;
        [SerializeField] private T m_hardestValue;

        protected T EasiestValue => m_easiestValue;
        protected T HardestValue => m_hardestValue;
        
        [SerializeField] private AnimationCurve m_difficultyCurve = AnimationCurve.Linear(0, 0, 1, 1);

        protected AnimationCurve DifficultyCurve => m_difficultyCurve;

        public DifficultyAdjustedValue(T _easiestValue, T _hardestValue)
        {
            m_easiestValue = _easiestValue;
            m_hardestValue = _hardestValue;
        }

        public T GetValue(float _difficulty)
        {
            var adjustedDifficulty = DifficultyCurve.Evaluate(_difficulty);
            return GetAdjustedValue(adjustedDifficulty);
        }

        protected abstract T GetAdjustedValue(float _difficulty);
    }

    [Serializable]
    public class DifficultyAdjustedFloat : DifficultyAdjustedValue<float>
    {
        public DifficultyAdjustedFloat(float _easiestValue, float _hardestValue)
            : base(_easiestValue, _hardestValue) { }
        
        protected override float GetAdjustedValue(float _difficulty)
            => Mathf.Lerp(EasiestValue, HardestValue, _difficulty);
    }

    [Serializable]
    public class DifficultyAdjustedInteger : DifficultyAdjustedValue<int>
    {
        public DifficultyAdjustedInteger(int _easiestValue, int _hardestValue)
            : base(_easiestValue, _hardestValue) { }

        protected override int GetAdjustedValue(float _difficulty)
            => (int)((HardestValue - EasiestValue) * _difficulty) + EasiestValue;
    }
}