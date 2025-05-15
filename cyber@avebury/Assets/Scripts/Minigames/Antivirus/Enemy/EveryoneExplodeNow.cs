using System.Collections.Generic;
using CyberAvebury.Minigames;
using UnityEngine;

namespace CyberAvebury
{
    public class EveryoneExplodeNow : MonoBehaviour
    {
        private Minigame m_minigame;
        
        private EnemySpawner m_enemies;

        private void Awake()
        {
            m_minigame = GetComponentInParent<Minigame>();
            
            m_enemies = m_minigame.GetComponentInChildren<EnemySpawner>();
            
            m_minigame.OnPassed.AddListener(OnMinigamePassed);
            m_minigame.OnFailed.AddListener(OnMinigameFailed);
        }

        private void OnMinigamePassed()
        {
            var enemies = new List<Enemy>(m_enemies.Enemies);
            foreach (var enemy in enemies)
            {
                enemy.Explode();
            }
        }

        private void OnMinigameFailed()
        {
            foreach (var enemy in m_enemies.Enemies)
            {
                enemy.FlyOff();
            }
        }
    }
}