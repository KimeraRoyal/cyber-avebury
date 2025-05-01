using System.Collections;
using UnityEngine;

namespace CyberAvebury
{
    public class BossSwitcher : MonoBehaviour
    {
        private Dialogue m_dialogue;

        private Antivirus m_antivirus;

        private EnemySpawner m_enemySpawner;

        [SerializeField] private DialogueLineObjectBase m_switchDialogue;
        
        [SerializeField] private GameObject[] m_disableOnSwitch;
        [SerializeField] private GameObject[] m_enableOnSwitch;

        [SerializeField] private int m_targetScore = 5;
        [SerializeField] private float m_scoreResetDelay = 0.1f;
        
        private bool m_switched;

        private void Awake()
        {
            m_dialogue = FindAnyObjectByType<Dialogue>();
            
            m_antivirus = GetComponentInParent<Antivirus>();

            m_enemySpawner = m_antivirus.GetComponentInChildren<EnemySpawner>();
            
            m_antivirus.OnScoreUpdated.AddListener(OnScoreUpdated);
        }

        private void OnScoreUpdated(int _score)
        {
            if(m_switched || _score < m_targetScore) { return; }
            m_switched = true;
            
            foreach(var disableObject in m_disableOnSwitch)
            {
                disableObject.SetActive(false);
            }

            foreach (var enemy in m_enemySpawner.Enemies)
            {
                enemy.FlyOff();
            }
            m_enemySpawner.Pause = true;

            StartCoroutine(WaitForDialogue());
        }

        private IEnumerator WaitForDialogue()
        {
            var score = m_antivirus.TargetScore;
            for (var i = 0; i < score; i++)
            {
                m_antivirus.ChangeScore(-1);
                yield return new WaitForSeconds(m_scoreResetDelay);
            }
            
            m_dialogue.AddLine(m_switchDialogue.GetLine());
            yield return new WaitUntil(() => !m_dialogue.HasDialogue);
            
            foreach(var enableObject in m_enableOnSwitch)
            {
                enableObject.SetActive(true);
            }
        }
    }
}
