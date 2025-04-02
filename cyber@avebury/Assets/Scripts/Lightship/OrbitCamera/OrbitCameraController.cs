// Copyright 2019 Niantic, Inc. All Rights Reserved.

using System;
using DG.Tweening;
using Niantic.Lightship.Maps.SampleAssets.Cameras.OrbitCamera.Internal;
using Niantic.Lightship.Maps.SampleAssets.Cameras.OrbitCamera.Internal.Input;
using Niantic.Lightship.Maps.SampleAssets.Cameras.OrbitCamera.Internal.Input.Gestures;
using Niantic.Lightship.Maps.SampleAssets.Cameras.OrbitCamera.Internal.ZoomCurves;
using UnityEngine;

namespace Niantic.Lightship.Maps.SampleAssets.Cameras.OrbitCamera
{
    /// <summary>
    /// Camera controller for Niantic-standard map camera
    /// interactions, similar to the Pokemon GO camera.
    /// </summary>
    public class OrbitCameraController : MonoBehaviour
    {
        [SerializeField]
        private float _minimumZoomDistance = 23f;

        [SerializeField]
        private float _maximumZoomDistance = 99f;

        [SerializeField]
        private float _minimumPitchDegrees = 20.0f;

        [SerializeField]
        private float _maximumPitchDegrees = 60.0f;

        [SerializeField]
        private float _verticalFocusOffset = 10.0f;

        [SerializeField]
        private GestureSettings _gestureSettings;

        [SerializeField]
        private Camera _camera;

        [SerializeField]
        private GameObject _focusObject;

        [SerializeField] private Transform m_focusOverride;
        [SerializeField] private float m_focusOverrideDistance = 10.0f;
        [SerializeField] private Vector3 m_focusOverrideOffset;

        [SerializeField] private float m_focusBlendTweenDuration = 1.0f;
        [SerializeField] private Ease m_focusBlendTweenEase = Ease.Linear;

        private InputService _inputService;
        private CameraGestureTracker _gestureTracker;
        private IZoomCurveEvaluator _zoomCurveEvaluator;

        [SerializeField] [Range(0.0f, 1.0f)] private float m_focusBlend;
        private Sequence m_focusBlendSequence;
        
        private Vector3 m_orbitPosition;
        private Quaternion m_orbitRotation;

        private Vector3 m_focusPosition;
        private Quaternion m_focusRotation;

        public void Awake()
        {
            _gestureTracker = new CameraGestureTracker(_camera, _focusObject, _gestureSettings);
            _inputService = new InputService(_gestureTracker);

            _zoomCurveEvaluator = new ZoomCurveEvaluator(
                _minimumZoomDistance,
                _maximumZoomDistance,
                _minimumPitchDegrees,
                _maximumPitchDegrees,
                _verticalFocusOffset);
        }

        public void Update()
        {
            _inputService.Update();
        }

        // Late update to ensure we use the latest avatar position
        private void LateUpdate()
        {
            CalculateOrbitValues();
            CalculateFocusValues();

            var position = m_orbitPosition;
            var rotation = m_orbitRotation;

            if (m_focusBlend > 0.0001f)
            {
                position = Vector3.Lerp(m_orbitPosition, m_focusPosition, m_focusBlend);
                rotation = Quaternion.Lerp(m_orbitRotation, m_focusRotation, m_focusBlend);
            }

            _camera.transform.position = position;
            _camera.transform.rotation = rotation;
        }

        private void CalculateOrbitValues()
        {
            float rotationAngleDegrees = _gestureTracker.RotationAngleDegrees;
            float rotationAngleRadians = Mathf.Deg2Rad * rotationAngleDegrees;
            float zoomFraction = _gestureTracker.ZoomFraction;

            float distance = _zoomCurveEvaluator.GetDistanceFromZoomFraction(zoomFraction);
            float elevMeters = _zoomCurveEvaluator.GetElevationFromDistance(distance);
            float pitchDegrees = _zoomCurveEvaluator.GetAngleFromDistance(distance);

            // Position the camera above the x-z plane,
            // according to our pitch and distance constraints.
            float x = -distance * Mathf.Sin(rotationAngleRadians);
            float z = -distance * Mathf.Cos(rotationAngleRadians);
            var offsetPos = new Vector3(x, elevMeters, z);

            m_orbitPosition = _focusObject.transform.position + offsetPos;
            m_orbitRotation = Quaternion.Euler(pitchDegrees, rotationAngleDegrees, 0.0f);
        }

        private void CalculateFocusValues()
        {
            if(!m_focusOverride) { return; }
            
            float rotationAngleDegrees = _gestureTracker.RotationAngleDegrees;
            float rotationAngleRadians = Mathf.Deg2Rad * rotationAngleDegrees;

            float elevMeters = _zoomCurveEvaluator.GetElevationFromDistance(m_focusOverrideDistance);
            float pitchDegrees = _zoomCurveEvaluator.GetAngleFromDistance(m_focusOverrideDistance);

            // Position the camera above the x-z plane,
            // according to our pitch and distance constraints.
            float x = -m_focusOverrideDistance * Mathf.Sin(rotationAngleRadians);
            float z = -m_focusOverrideDistance * Mathf.Cos(rotationAngleRadians);
            var offsetPos = new Vector3(x, elevMeters, z);

            m_focusPosition = m_focusOverride.transform.position + offsetPos + m_focusOverrideOffset;
            m_focusRotation = Quaternion.Euler(pitchDegrees, rotationAngleDegrees, 0.0f);
        }

        public void SetFocus(Transform _focus)
        {
            if (m_focusBlendSequence is { active: true })
            {
                m_focusBlendSequence.Kill();
            }

            if (_focus)
            {
                m_focusOverride = _focus;
            }

            m_focusBlendSequence = DOTween.Sequence();
            m_focusBlendSequence.Append(DOTween.To(
                () => m_focusBlend,
                _value => m_focusBlend = _value,
                _focus ? 1.0f : 0.0f,
                m_focusBlendTweenDuration).SetEase(m_focusBlendTweenEase));
            if (!_focus)
            {
                m_focusBlendSequence.AppendCallback(() => m_focusOverride = null);
            }
        }
    }
}
