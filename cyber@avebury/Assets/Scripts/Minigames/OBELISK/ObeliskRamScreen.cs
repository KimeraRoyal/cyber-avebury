using System;
using KR;
using UnityEngine;

namespace CyberAvebury
{
    [RequireComponent(typeof(FrameAnimator))]
    public class ObeliskRamScreen : MonoBehaviour
    {
        private enum ScreenState
        {
            Default,
            Pass,
            Fail,
            Warning
        }
        
        private Obelisk m_obelisk;
        
        private FrameAnimator m_animator;
        
        [SerializeField] private FrameAnimation m_defaultFace;
        [SerializeField] private FrameAnimation m_warningFace;
        [SerializeField] private FrameAnimation m_subgamePassedFace;
        [SerializeField] private FrameAnimation m_subgameFailedFace;
        
        [SerializeField] private float m_faceHoldDuration = 1.0f;

        private ScreenState m_state;
        private float m_faceHoldTimer;

        private void Awake()
        {
            m_obelisk = GetComponentInParent<Obelisk>();

            m_animator = GetComponent<FrameAnimator>();
            
            m_obelisk.OnBeginLoadingSubgame.AddListener(OnBeginLoadingSubgame);
            m_obelisk.OnSubgamePassed.AddListener(OnSubgamePassed);
            m_obelisk.OnSubgameFailed.AddListener(OnSubgameFailed);
        }

        private void Update()
        {
            if(m_state is ScreenState.Default or ScreenState.Warning) { return; }

            m_faceHoldTimer += Time.deltaTime;
            if(m_faceHoldTimer < m_faceHoldDuration) { return; }

            ChangeState(ScreenState.Default);
        }

        private void OnBeginLoadingSubgame(int _subgameIndex)
            => ChangeState(ScreenState.Warning);

        private void OnSubgamePassed(int _subgameIndex)
            => ChangeState(ScreenState.Pass);

        private void OnSubgameFailed(int _subgameIndex)
            => ChangeState(ScreenState.Fail);

        private void ChangeState(ScreenState _state)
        {
            m_state = _state;
            m_animator.Animation = m_state switch
            {
                ScreenState.Default => m_defaultFace,
                ScreenState.Pass => m_subgamePassedFace,
                ScreenState.Fail => m_subgameFailedFace,
                ScreenState.Warning => m_warningFace,
                _ => throw new ArgumentOutOfRangeException()
            };
            m_faceHoldTimer = 0.0f;
        }
    }
}
