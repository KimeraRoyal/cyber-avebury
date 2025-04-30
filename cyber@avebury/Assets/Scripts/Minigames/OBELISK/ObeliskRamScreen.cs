using KR;
using UnityEngine;

namespace CyberAvebury
{
    [RequireComponent(typeof(FrameAnimator))]
    public class ObeliskRamScreen : MonoBehaviour
    {
        private Obelisk m_obelisk;
        
        private FrameAnimator m_animator;
        
        [SerializeField] private FrameAnimation m_defaultFace;
        [SerializeField] private FrameAnimation m_subgamePassedFace;
        [SerializeField] private FrameAnimation m_subgameFailedFace;
        
        [SerializeField] private float m_faceHoldDuration = 1.0f;
        private float m_faceHoldTimer;
        private bool m_hasExpression;

        private void Awake()
        {
            m_obelisk = GetComponentInParent<Obelisk>();

            m_animator = GetComponent<FrameAnimator>();
            
            m_obelisk.OnSubgamePassed.AddListener(OnSubgamePassed);
            m_obelisk.OnSubgameFailed.AddListener(OnSubgameFailed);
        }

        private void Update()
        {
            if(!m_hasExpression) { return; }

            m_faceHoldTimer += Time.deltaTime;
            if(m_faceHoldTimer < m_faceHoldDuration) { return; }

            m_animator.Animation = m_defaultFace;
            m_hasExpression = false;
        }

        private void OnSubgamePassed(int _subgameIndex)
            => HoldFace(m_subgamePassedFace);

        private void OnSubgameFailed(int _subgameIndex)
            => HoldFace(m_subgameFailedFace);

        private void HoldFace(FrameAnimation _face)
        {
            m_animator.Animation = _face;
            m_faceHoldTimer = 0.0f;
            m_hasExpression = true;
        }
    }
}
