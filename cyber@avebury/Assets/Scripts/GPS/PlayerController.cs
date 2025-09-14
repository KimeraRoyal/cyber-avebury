using System.Collections;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.Events;

namespace CyberAvebury
{
    public class PlayerController : MonoBehaviour
    {
        public enum GPSState
        {
            Default,
            Initialising,
            Failed,
            Active,
            Disconnected
        }
        
        private GPS m_gps;

        [SerializeField] private float m_accuracy = 10.0f;
        [SerializeField] private float m_updateDistance = 10.0f;

        [SerializeField] private float m_movementDurationPerMeter = 1.0f;
        [SerializeField] private float m_lookDurationPercentage = 1.0f;
        [SerializeField] private float m_maxTweenDistance = 100.0f;
        
        private GPSState m_state;
        private double m_lastUpdateTime;

        private Sequence m_movementSequence;

#if DEBUG
        [SerializeField] private LatLng m_debugPosition;

        [SerializeField] private Vector3 m_moveBy;

        [SerializeField] private float m_movementSpeed = 1.0f;
        [SerializeField] private float m_rotationSpeed = 1.0f;
#endif

        public GPSState State => m_state;

        public UnityEvent<LocationInfo> OnLocationInfoUpdated;
        public UnityEvent<LatLng> OnLocationUpdated;
        public UnityEvent<Vector3> OnWorldPositionUpdated;

        private void Awake()
        {
            m_gps = FindAnyObjectByType<GPS>();
        }

        private void Start()
        {
#if DEBUG
            transform.position = m_gps.GetScenePosition(m_debugPosition);
#endif
            StartCoroutine(GpsMovement());
        }

        private void Update()
        {
            KeyboardMovement();
        }

        private IEnumerator GpsMovement()
        {
            if (Application.isEditor)
            {
                m_state = GPSState.Active;
                yield break;
            }

            yield return AuthorizePermission(Permission.FineLocation);
            if (!Input.location.isEnabledByUser)
            {
                Debug.LogError("User has location services disabled.");
                m_state = GPSState.Failed;
                yield break;
            }
            
            Input.location.Start(m_accuracy, m_updateDistance);
            m_state = GPSState.Initialising;
            
            var maxWait = 20;
            while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
            {
                yield return new WaitForSeconds(1);
                maxWait--;
            }
            
            if (maxWait < 1)
            {
                Debug.LogError("Location services timed out.");
                m_state = GPSState.Failed;
                yield break;
            }
            
            if (Input.location.status == LocationServiceStatus.Failed)
            {
                Debug.LogError("Location services failed to initialise.");
                m_state = GPSState.Failed;
                yield break;
            }
            
            var gpsInfo = Input.location.lastData;
            var location = new LatLng(gpsInfo.latitude, gpsInfo.longitude);
            UpdateLocation(location, true);
            
            m_state = GPSState.Active;
            while (isActiveAndEnabled)
            {
                gpsInfo = Input.location.lastData;
                if (gpsInfo.timestamp > m_lastUpdateTime)
                {
                    m_lastUpdateTime = gpsInfo.timestamp;
                    location = new LatLng(gpsInfo.latitude, gpsInfo.longitude);
                    UpdateLocation(location);
                    OnLocationInfoUpdated?.Invoke(gpsInfo);
                }

                yield return null;
            }
            Input.location.Stop();
            m_state = GPSState.Disconnected;
        }

        private void UpdateLocation(LatLng _coordinates, bool _immediate = false)
        {
            OnLocationUpdated?.Invoke(_coordinates);
            
            var position = m_gps.GetScenePosition(_coordinates);
            MoveTo(position, _immediate);
            OnWorldPositionUpdated?.Invoke(position);
        }

        [Button("Move")]
        public void DebugMove()
        {
#if DEBUG
            MoveTo(transform.position + m_moveBy);
            OnWorldPositionUpdated?.Invoke(m_moveBy);
#endif
        }

        private void MoveTo(Vector3 _targetPosition, bool _immediate = false)
        {
            if(m_movementSequence is { active: true }) { m_movementSequence.Kill(); }

            var difference = _targetPosition - transform.position;
            var distance = difference.magnitude;
            
            if (_immediate || distance > m_maxTweenDistance)
            {
                transform.position = _targetPosition;
                return;
            }
            
            var duration = m_movementDurationPerMeter * distance;

            m_movementSequence = DOTween.Sequence();
            m_movementSequence.Append(transform.DOMove(_targetPosition, duration)); 
            m_movementSequence.Insert(0.0f, transform.DOLookAt(_targetPosition, duration * m_lookDurationPercentage)); 
        }

        private void KeyboardMovement()
        {
#if DEBUG
            var movement = transform.forward * Input.GetAxis("Vertical");
            var rotation = Vector3.up * Input.GetAxis("Horizontal");
            transform.position += movement * (m_movementSpeed * Time.deltaTime);
            transform.eulerAngles += rotation * (m_rotationSpeed * Time.deltaTime);
#endif
        }

        private IEnumerator AuthorizePermission(string _permission)
        {
#if UNITY_ANDROID
            if (Permission.HasUserAuthorizedPermission(_permission)) { yield break; }
            
            Permission.RequestUserPermission(_permission);
            while (!Permission.HasUserAuthorizedPermission(_permission))
            {
                yield return new WaitForSeconds(1.0f);
            }
#endif
            yield return null;
        }
    }
}
